<template>
  <div>
    <h2>Dishes</h2>

    <!-- Admin Add Dish Button -->
    <div v-if="isAdmin" class="admin-add">
      <button @click="showAddForm = !showAddForm">
        {{ showAddForm ? 'Cancel' : 'Add New Dish' }}
      </button>

      <div v-if="showAddForm" class="add-form">
        <input v-model="newDish.name" placeholder="Name" />
        <input v-model="newDish.description" placeholder="Description" />
        <input v-model="newDish.price" type="number" placeholder="Price" />
         <input 
                  type="file" 
                  @change="onImageChange($event)" 
                  accept="image/*" 
                  class="editable-input"
                />        <button @click="addDish">Submit</button>
      </div>
    </div>

    <!-- Search input -->
    <input 
      v-model="searchQuery" 
      type="text" 
      placeholder="Search dishes..." 
      @input="searchDishes"
    />

    <!-- Dishes Table -->
    <div v-if="filteredDishes && filteredDishes.length > 0">
      <table class="dishes-table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Price</th>
            <th>Image</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="dish in filteredDishes" :key="dish.id" @click="editDish(dish)">
            <td>
              <input 
                v-if="editingDish && editingDish.id === dish.id" 
                :value="editingDish.name"
                @input="editingDish.name = $event.target.value"
                class="editable-input"
              />
              <span v-else>{{ dish.name }}</span>
            </td>
            <td>
              <input 
                v-if="editingDish && editingDish.id === dish.id" 
                :value="editingDish.description"
                @input="editingDish.description = $event.target.value"
                class="editable-input"
              />
              <span v-else>{{ dish.description }}</span>
            </td>
            <td>
              <input 
                v-if="editingDish && editingDish.id === dish.id" 
                :value="editingDish.price"
                @input="editingDish.price = $event.target.value"
                class="editable-input"
              />
              <span v-else>{{ dish.price }}</span>
            </td>
            <td>
              <div v-if="editingDish && editingDish.id === dish.id">
                <!-- Image Preview -->
                <input 
                  type="file" 
                  @change="onImageChange($event)" 
                  accept="image/*" 
                  class="editable-input"
                />
                <img v-if="imagePreview" :src="imagePreview" alt="Image preview" class="image-preview"/>
              </div>
              <span v-else><img :src="dish.image" alt="Dish Image" class="menu-item-image" /></span>
            </td>
            <td>
              <button v-if="editingDish && editingDish.id === dish.id" @click.stop="saveDish(dish)">Save</button>
              <button v-else @click.stop="editDish(dish)">Edit</button>
              <button @click.stop="deleteDish(dish.id)">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-else>
      <p>No dishes available.</p>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from "vue";
import { useAuthStore } from "../stores/authStore";
import { getMainAPI } from "../stores/plugins/axios";
import Swal from 'sweetalert2';


export default {
  name: "Dishes",
  setup() {
    const dishes = ref([]);
    const filteredDishes = ref([]);
    const searchQuery = ref("");
    const showAddForm = ref(false);
    const authStore = useAuthStore();
    const isAdmin = ref(false);

    const newDish = ref({
      name: "",
      description: "",
      price: 0,
      image: ""
    });

    const editingDish = ref(null); // Track the dish being edited
    const imagePreview = ref(null); // Preview the selected image

    const fetchDishes = async () => {
      try {
        const response = await getMainAPI().get("/Dishes");
        dishes.value = response.data;
        filteredDishes.value = dishes.value;
      } catch (error) {
        console.log(error);
        Swal.error("Error fetching dishes:", error);
      }
    };

    const addDish = async () => {
      try {
        const response = await getMainAPI().post("/Dishes", newDish.value);
        dishes.value.push(response.data);
        filteredDishes.value = dishes.value;
        newDish.value = { name: "", description: "", price: 0, image: "" };
        showAddForm.value = false;
        Swal.fire("Dish added successfully!");
      } catch (error) {
        console.error("Error adding dish:", error);
        Swal.fire("Error adding dish.");
      }
    };

    const deleteDish = async (id) => {
      const result = await Swal.fire({
        title: 'Are you sure?',
        text: 'You won’t be able to revert this!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
      });

      if (result.isConfirmed) {
        try {
          let result =  await getMainAPI().delete(`/Dishes/delete/${id}`);
          dishes.value = dishes.value.filter(dish => dish.id !== id);
          filteredDishes.value = dishes.value;

          Swal.fire("Deleted", `${result.Message}!`, "success");
        } catch (error) {
          console.error("Error deleting dish:", error);
          Swal.fire("Error", "Failed to delete dish.", "error");
        }
      }
    };

    const editDish = (dish) => {
      editingDish.value = { ...dish }; 
      imagePreview.value = dish.image; 
    };

    const saveDish = async () => {
      try {
        if (imagePreview.value) {
          editingDish.value.image = imagePreview.value; 
        }
        await getMainAPI().put(`/Dishes/${editingDish.value.id}`, editingDish.value);
        const index = dishes.value.findIndex(d => d.id === editingDish.value.id);
        if (index !== -1) {
          dishes.value[index] = editingDish.value;
          filteredDishes.value = [...dishes.value];
        }
        editingDish.value = null; 
        imagePreview.value = null; 
        Swal.fire("Dish updated successfully.");
      } catch (error) {
        console.error("Error updating dish:", error);
        Swal.fire("Failed to update dish.");
      }
    };

    const onImageChange = (event) => {
      const file = event.target.files[0];
      if (file) {
        const reader = new FileReader();
        reader.onload = () => {
          imagePreview.value = reader.result; // Preview the image
        };
        reader.readAsDataURL(file);
      }
    };

    const searchDishes = () => {
      if (searchQuery.value.trim() === "") {
        filteredDishes.value = dishes.value;
      } else {
        filteredDishes.value = dishes.value.filter(dish =>
          dish.name.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
          dish.description.toLowerCase().includes(searchQuery.value.toLowerCase())
        );
      }
    };

    onMounted(() => {
      console.log(authStore.decodedToken);
      isAdmin.value = authStore.user && authStore.role === "Admin";
      fetchDishes();
    });

    return {
      dishes,
      filteredDishes,
      isAdmin,
      searchQuery,
      searchDishes,
      showAddForm,
      newDish,
      addDish,
      deleteDish,
      editDish,
      saveDish,
      editingDish,
      imagePreview,
      onImageChange
    };
  }
};
</script>
<style scoped>
.menu-item-image, .image-preview {
  width: 100px;  /* Set a fixed width */
  height: 100px; /* Set a fixed height */
  object-fit: cover; /* Ensures the image is resized and cropped proportionally */
}
</style>
