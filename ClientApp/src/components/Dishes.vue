<template>
  <div class="container mt-5">
    <h2>Dishes</h2>

    <!-- Admin Add Dish Button -->
    <div v-if="isAdmin" class="admin-add mb-3">
      <button class="btn btn-primary" @click="showAddForm = !showAddForm">
        {{ showAddForm ? 'Cancel' : 'Add New Dish' }}
      </button>

      <div v-if="showAddForm" class="mt-3">
        <div class="mb-3">
          <label for="dishName" class="form-label">Name</label>
          <input v-model="newDish.name" id="dishName" class="form-control" placeholder="Enter dish name" />
        </div>
        <div class="mb-3">
          <label for="dishDescription" class="form-label">Description</label>
          <input v-model="newDish.description" id="dishDescription" class="form-control" placeholder="Enter dish description" />
        </div>
        <div class="mb-3">
          <label for="dishPrice" class="form-label">Price</label>
          <input v-model="newDish.price" id="dishPrice" type="number" class="form-control" placeholder="Enter dish price" />
        </div>
        <div class="mb-3">
          <label for="dishImage" class="form-label">Dish Image</label>
          <input type="file" @change="onImageChange($event)" accept="image/*" class="form-control" />
        </div>
        <button @click="addDish" class="btn btn-success">Submit</button>
      </div>
    </div>

    <!-- Search input -->
    <div class="mb-3">
      <input 
        v-model="searchQuery" 
        type="text" 
        placeholder="Search dishes..." 
        @input="searchDishes"
        class="form-control"
      />
    </div>

    <!-- Dishes Table -->
    <div v-if="filteredDishes && filteredDishes.length > 0">
      <table class="table table-striped">
        <thead>
          <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Price</th>
            <th>Image</th>
            <th v-if="isAdmin">Actions</th>
            <th v-if="!isAdmin">Review & Rate</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="dish in filteredDishes" :key="dish.id">
            <td>
              <input 
                v-if="editingDish && editingDish.id === dish.id" 
                :value="editingDish.name"
                @input="editingDish.name = $event.target.value"
                class="form-control"
              />
              <span v-else>{{ dish.name }}</span>
            </td>
            <td>
              <input 
                v-if="editingDish && editingDish.id === dish.id" 
                :value="editingDish.description"
                @input="editingDish.description = $event.target.value"
                class="form-control"
              />
              <span v-else>{{ dish.description }}</span>
            </td>
            <td>
              <input 
                v-if="editingDish && editingDish.id === dish.id" 
                :value="editingDish.price"
                @input="editingDish.price = $event.target.value"
                class="form-control"
              />
              <span v-else>{{ dish.price }}</span>
            </td>
            <td>
              <div v-if="editingDish && editingDish.id === dish.id">
                <input 
                  type="file" 
                  @change="onImageChange($event)" 
                  accept="image/*" 
                  class="form-control"
                />
                <img v-if="imagePreview" :src="imagePreview" alt="Image preview" class="img-thumbnail mt-2" />
              </div>
              <span v-else><img :src="dish.image" alt="Dish Image" class="img-thumbnail" style="width: 100px; height: 100px; object-fit: cover;" /></span>
            </td>
            <td v-if="isAdmin">
              <button v-if="editingDish && editingDish.id === dish.id" @click.stop="saveDish(dish)" class="btn btn-warning btn-sm">Save</button>
              <button v-else @click.stop="editDish(dish)" class="btn btn-secondary btn-sm">Edit</button>
              <button @click.stop="deleteDish(dish.id)" class="btn btn-danger btn-sm">Delete</button>
            </td>
            <td v-if="!isAdmin">
              <div class="mb-3">
                <label for="review" class="form-label">Your Review</label>
                <textarea v-model="dish.reviewText" class="form-control" rows="3" placeholder="Write your review here..."></textarea>
              </div>
              <div class="mb-3">
                <label for="rating" class="form-label">Rate (1-6)</label>
                <input 
                  type="number" 
                  v-model="dish.rating" 
                  min="1" max="6" 
                  class="form-control"
                  placeholder="Rate from 1 to 6"
                />
              </div>
              <button @click.stop="saveReview(dish)" class="btn btn-primary btn-sm">Save Review</button>
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

    const saveReview = async (dish) => {
  try {
    const reviewPayload = {
      dishId: dish.id,
      customerName: authStore.decodedToken.sub,
      rating:dish.rating, // Assuming you have the user ID from the token
      reviewText: dish.reviewText,
    };
    console.log(reviewPayload);
    if (!reviewPayload.rating || !reviewPayload.reviewText) {
      Swal.fire("Please complete both rating and review text.");
      return;
    }

    await getMainAPI().post("/Review", reviewPayload);
    Swal.fire("Review saved successfully.");
  

  } catch (error) {
    console.error("Error saving review:", error);
    Swal.fire("Failed to save review.");
  }
};
    const searchDishes = () => {
      if (searchQuery.value.trim() === "") {
        filteredDishes.value = dishes.value;
      } else {
        filteredDishes.value = dishes.value.filter(dish => 
          dish.name.toLowerCase().includes(searchQuery.value.toLowerCase())
        );
      }
    };

    onMounted(async () => {
      await fetchDishes();
      isAdmin.value = authStore.decodedToken.Roles
      .split(',')                // [ "FrontEnd", "Admin", "User" ]
      .map(r => r.trim())      
      .includes('Admin');      });

    return {
      dishes,
      filteredDishes,
      searchQuery,
      showAddForm,
      newDish,
      editingDish,
      imagePreview,
      isAdmin,
      fetchDishes,
      addDish,
      deleteDish,
      editDish,
      saveDish,
      onImageChange,
      saveReview,
      searchDishes,
    };
  },
};
</script>

<style scoped>
.admin-add {
  margin-bottom: 20px;
}
</style>
