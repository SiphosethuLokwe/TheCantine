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
        <input v-model="newDish.image" placeholder="Image URL" />
        <button @click="addDish">Submit</button>
      </div>
    </div>

    <!-- Search input -->
    <input 
      v-model="searchQuery" 
      type="text" 
      placeholder="Search dishes..." 
      @input="searchDishes"
    />

    <div v-if="filteredDishes && filteredDishes.length > 0">
      <div v-for="dish in filteredDishes" :key="dish.id" class="menu-item">
        <img :src="dish.image" alt="Dish Image" class="menu-item-image" />
        <div class="menu-item-details">
          <h3>{{ dish.name }}</h3>
          <p>{{ dish.description }}</p>
          <p><strong>Price: ${{ dish.price }}</strong></p>

          <!-- Show rating section for Frontend Users -->
          <div v-if="!isAdmin">
            <label for="rating">Rate this Dish:</label>
            <select v-model="dish.rating">
              <option value="1">1</option>
              <option value="2">2</option>
              <option value="3">3</option>
              <option value="4">4</option>
              <option value="5">5</option>
            </select>
            <button @click="rateDish(dish)">Submit Rating</button>
          </div>

          <!-- Show ratings and controls for Admin -->
          <div v-if="isAdmin">
            <p><strong>Ratings:</strong></p>
            <ul>
              <li v-for="rating in dish.ratings" :key="rating.userId">
                User {{ rating.userId }} rated: {{ rating.value }} stars
              </li>
            </ul>

            <div class="admin-controls">
              <button @click="editDish(dish)">Edit</button>
              <button @click="deleteDish(dish.id)">Delete</button>
            </div>
          </div>
        </div>
      </div>
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

    const fetchDishes = async () => {
      try {
        const response = await getMainAPI().get("/Dishes");
        dishes.value = response.data;
        filteredDishes.value = dishes.value;
      } catch (error) {
        console.error("Error fetching dishes:", error);
      }
    };

    const addDish = async () => {
      try {
        const response = await getMainAPI().post("/Dishes", newDish.value);
        dishes.value.push(response.data);
        filteredDishes.value = dishes.value;
        newDish.value = { name: "", description: "", price: 0, image: "" };
        showAddForm.value = false;
        alert("Dish added successfully!");
      } catch (error) {
        console.error("Error adding dish:", error);
        alert("Error adding dish.");
      }
    };

    const deleteDish = async (id) => {
      try {
        await getMainAPI().delete(`/Dishes/${id}`);
        dishes.value = dishes.value.filter(dish => dish.id !== id);
        filteredDishes.value = dishes.value;
        alert("Dish deleted successfully.");
      } catch (error) {
        console.error("Error deleting dish:", error);
        alert("Failed to delete dish.");
      }
    };

    const editDish = (dish) => {
      const updatedName = prompt("Edit name", dish.name);
      const updatedDescription = prompt("Edit description", dish.description);
      const updatedPrice = prompt("Edit price", dish.price);
      const updatedImage = prompt("Edit image URL", dish.image);

      if (updatedName && updatedDescription && updatedPrice && updatedImage) {
        updateDish({
          ...dish,
          name: updatedName,
          description: updatedDescription,
          price: Number(updatedPrice),
          image: updatedImage
        });
      }
    };

    const updateDish = async (dish) => {
      try {
        await getMainAPI().put(`/Dishes/${dish.id}`, dish);
        const index = dishes.value.findIndex(d => d.id === dish.id);
        if (index !== -1) {
          dishes.value[index] = dish;
          filteredDishes.value = [...dishes.value];
        }
        alert("Dish updated successfully.");
      } catch (error) {
        console.error("Error updating dish:", error);
        alert("Failed to update dish.");
      }
    };

    const rateDish = async (dish) => {
      try {
        const ratingData = {
          userId: authStore.user.id,
          rating: dish.rating
        };
        await getMainAPI().post(`/dishes/${dish.id}/rate`, ratingData);
        alert("Rating submitted successfully!");
      } catch (error) {
        console.error("Error submitting rating:", error);
        alert("Error submitting rating.");
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
      isAdmin.value = authStore.user && authStore.user.role === "Admin";
      fetchDishes();
    });

    return {
      dishes,
      filteredDishes,
      rateDish,
      isAdmin,
      searchQuery,
      searchDishes,
      showAddForm,
      newDish,
      addDish,
      deleteDish,
      editDish,
      updateDish
    };
  }
};
</script>

<style scoped>
.menu-item {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
}

.menu-item-image {
  width: 100px;
  height: 100px;
  object-fit: cover;
  margin-right: 20px;
}

.menu-item-details {
  flex: 1;
}

button {
  margin-top: 10px;
  margin-right: 10px;
}

.admin-add {
  margin-bottom: 20px;
}

.add-form input {
  display: block;
  margin-bottom: 10px;
  padding: 5px;
  width: 250px;
}

.admin-controls {
  margin-top: 10px;
}
</style>
