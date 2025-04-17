<template>
  <div>
    <h2>Drinks</h2>

    <!-- Admin Add Drink Button -->
    <div v-if="isAdmin" class="admin-add">
      <button @click="showAddForm = !showAddForm">
        {{ showAddForm ? 'Cancel' : 'Add New Drink' }}
      </button>

      <div v-if="showAddForm" class="add-form">
        <input v-model="newDrink.name" placeholder="Name" />
        <input v-model="newDrink.description" placeholder="Description" />
        <input v-model="newDrink.price" type="number" placeholder="Price" />
         <input 
                  type="file" 
                  @change="onImageChange($event)" 
                  accept="image/*" 
                  class="editable-input"
                />        <button @click="addDrink">Submit</button>
      </div>
    </div>

    <!-- Search input -->
    <input 
      v-model="searchQuery" 
      type="text" 
      placeholder="Search Drinks..." 
      @input="searchDrinks"
    />

    <!-- Drinks Table -->
    <div v-if="filteredDrinks && filteredDrinks.length > 0">
      <table class="Drinks-table">
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
          <tr v-for="Drink in filteredDrinks" :key="Drink.id" @click="editDrink(Drink)">
            <td>
              <input 
                v-if="editingDrink && editingDrink.id === Drink.id" 
                :value="editingDrink.name"
                @input="editingDrink.name = $event.target.value"
                class="editable-input"
              />
              <span v-else>{{ Drink.name }}</span>
            </td>
            <td>
              <input 
                v-if="editingDrink && editingDrink.id === Drink.id" 
                :value="editingDrink.description"
                @input="editingDrink.description = $event.target.value"
                class="editable-input"
              />
              <span v-else>{{ Drink.description }}</span>
            </td>
            <td>
              <input 
                v-if="editingDrink && editingDrink.id === Drink.id" 
                :value="editingDrink.price"
                @input="editingDrink.price = $event.target.value"
                class="editable-input"
              />
              <span v-else>{{ Drink.price }}</span>
            </td>
            <td>
              <div v-if="editingDrink && editingDrink.id === Drink.id">
                <!-- Image Preview -->
                <input 
                  type="file" 
                  @change="onImageChange($event)" 
                  accept="image/*" 
                  class="editable-input"
                />
                <img v-if="imagePreview" :src="imagePreview" alt="Image preview" class="image-preview"/>
              </div>
              <span v-else><img :src="Drink.image" alt="Drink Image" class="menu-item-image" /></span>
            </td>
            <td>
              <button v-if="editingDrink && editingDrink.id === Drink.id" @click.stop="saveDrink(Drink)">Save</button>
              <button v-else @click.stop="editDrink(Drink)" class ="btn btn-warning">Edit</button>
              <button @click.stop="deleteDrink(Drink.id)"  class ="btn btn-danger">Delete</button>
              <button @click.stop="deleteDrink(Drink.id)"  class ="btn btn-info">View Ratings</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-else>
      <p>No Drinks available.</p>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from "vue";
import { useAuthStore } from "../stores/authStore";
import { getMainAPI } from "../stores/plugins/axios";
import Swal from 'sweetalert2';


export default {
  name: "Drinks",
  setup() {
    const Drinks = ref([]);
    const filteredDrinks = ref([]);
    const searchQuery = ref("");
    const showAddForm = ref(false);
    const authStore = useAuthStore();
    const isAdmin = ref(false);

    const newDrink = ref({
      name: "",
      description: "",
      price: 0,
      image: ""
    });

    const editingDrink = ref(null); // Track the Drink being edited
    const imagePreview = ref(null); // Preview the selected image

    const fetchDrinks = async () => {
      try {
        const response = await getMainAPI().get("/Drinks");
        Drinks.value = response.data;
        filteredDrinks.value = Drinks.value;
      } catch (error) {
        console.log(error);
        Swal.error("Error fetching Drinks:", error);
      }
    };

    const addDrink = async () => {
      try {
        const response = await getMainAPI().post("/Drinks", newDrink.value);
        Drinks.value.push(response.data);
        filteredDrinks.value = Drinks.value;
        newDrink.value = { name: "", description: "", price: 0, image: "" };
        showAddForm.value = false;
        Swal.fire("Drink added successfully!");
      } catch (error) {
        console.error("Error adding Drink:", error);
        Swal.fire("Error adding Drink.");
      }
    };

    const deleteDrink = async (id) => {
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
          let result =  await getMainAPI().delete(`/Drinks/delete/${id}`);
          Drinks.value = Drinks.value.filter(Drink => Drink.id !== id);
          filteredDrinks.value = Drinks.value;

          Swal.fire("Deleted", `${result.Message}!`, "success");
        } catch (error) {
          console.error("Error deleting Drink:", error);
          Swal.fire("Error", "Failed to delete Drink.", "error");
        }
      }
    };

    const editDrink = (Drink) => {
      editingDrink.value = { ...Drink }; 
      imagePreview.value = Drink.image; 
    };

    const saveDrink = async () => {
      try {
        if (imagePreview.value) {
          editingDrink.value.image = imagePreview.value; 
        }
        await getMainAPI().put(`/Drinks/${editingDrink.value.id}`, editingDrink.value);
        const index = Drinks.value.findIndex(d => d.id === editingDrink.value.id);
        if (index !== -1) {
          Drinks.value[index] = editingDrink.value;
          filteredDrinks.value = [...Drinks.value];
        }
        editingDrink.value = null; 
        imagePreview.value = null; 
        Swal.fire("Drink updated successfully.");
      } catch (error) {
        console.error("Error updating Drink:", error);
        Swal.fire("Failed to update Drink.");
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

    const searchDrinks = () => {
      if (searchQuery.value.trim() === "") {
        filteredDrinks.value = Drinks.value;
      } else {
        filteredDrinks.value = Drinks.value.filter(Drink =>
          Drink.name.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
          Drink.description.toLowerCase().includes(searchQuery.value.toLowerCase())
        );
      }
    };

    onMounted(() => {
      console.log(authStore.decodedToken);
      isAdmin.value = authStore.user && authStore.role === "Admin";
      fetchDrinks();
    });

    return {
      Drinks,
      filteredDrinks,
      isAdmin,
      searchQuery,
      searchDrinks,
      showAddForm,
      newDrink,
      addDrink,
      deleteDrink,
      editDrink,
      saveDrink,
      editingDrink,
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
