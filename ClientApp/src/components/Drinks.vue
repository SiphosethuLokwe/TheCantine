<template>
  <div>
    <h2>Drinks</h2>
    <!-- Search input -->
    <input 
      v-model="searchQuery" 
      type="text" 
      placeholder="Search drinks..." 
      @input="searchDrinks"
    />
    <div v-if="filteredDrinks && filteredDrinks.length > 0">
      <div v-for="drink in filteredDrinks" :key="drink.id" class="menu-item">
        <img :src="drink.image" alt="Drink Image" class="menu-item-image" />
        <div class="menu-item-details">
          <h3>{{ drink.name }}</h3>
          <p>{{ drink.description }}</p>
          <p><strong>Price: ${{ drink.price }}</strong></p>

          <!-- Show rating section for Frontend Users -->
          <div v-if="!isAdmin">
            <label for="rating">Rate this Drink:</label>
            <select v-model="drink.rating">
              <option value="1">1</option>
              <option value="2">2</option>
              <option value="3">3</option>
              <option value="4">4</option>
              <option value="5">5</option>
            </select>
            <button @click="rateDrink(drink)">Submit Rating</button>
          </div>

          <!-- Show ratings for Admin -->
          <div v-if="isAdmin">
            <p><strong>Ratings:</strong></p>
            <ul>
              <li v-for="rating in drink.ratings" :key="rating.userId">
                User {{ rating.userId }} rated: {{ rating.value }} stars
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
    <div v-else>
      <p>No drinks available.</p>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from "vue";
import { useAuthStore } from "../stores/authStore"; // Adjust the path as necessary
import { getMainAPI } from "../stores/plugins/axios";

export default {
  name: "Drinks",
  setup() {
    const drinks = ref([]);
    const filteredDrinks = ref([]);
    const searchQuery = ref("");
    const authStore = useAuthStore();
    const isAdmin = authStore.user && authStore.user.role === "Admin"; // Check if the user is Admin

    const fetchDrinks = async () => {
      try {
        const response = await getMainAPI().get("/Drinks"); // API to fetch drinks
        drinks.value = response.data;
        filteredDrinks.value = drinks.value; // Initially, show all drinks
      } catch (error) {
        console.error("Error fetching drinks:", error);
      }
    };

    const rateDrink = async (drink) => {
      try {
        const ratingData = {
          userId: authStore.user.id,
          rating: drink.rating
        };
        await getMainAPI().post(`/drinks/${drink.id}/rate`, ratingData);
        alert("Rating submitted successfully!");
      } catch (error) {
        console.error("Error submitting rating:", error);
        alert("Error submitting rating.");
      }
    };

    const searchDrinks = () => {
      if (searchQuery.value.trim() === "") {
        filteredDrinks.value = drinks.value; // No search term, show all drinks
      } else {
        filteredDrinks.value = drinks.value.filter(drink =>
          drink.name.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
          drink.description.toLowerCase().includes(searchQuery.value.toLowerCase())
        );
      }
    };

    onMounted(() => {
      fetchDrinks();
    });

    return {
      drinks,
      filteredDrinks,
      rateDrink,
      isAdmin,
      searchQuery,
      searchDrinks
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
}
</style>
