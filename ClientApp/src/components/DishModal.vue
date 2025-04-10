<template>
  <div class="modal fade" :id="modalId" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">{{ title }}</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
        </div>
        <div class="modal-body">
          <input v-model="localDish.name" class="form-control mb-2" placeholder="Name" />
          <input v-model="localDish.description" class="form-control mb-2" placeholder="Description" />
          <input v-model.number="localDish.price" class="form-control mb-2" type="number" placeholder="Price" />
          <input v-model="localDish.image" class="form-control mb-2" placeholder="Image URL" />
        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
          <button class="btn btn-primary" @click="submit">{{ submitText }}</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch, toRefs } from 'vue';

const props = defineProps({
  dish: Object,
  modalId: String,
  title: String,
  submitText: String
});

const emit = defineEmits(['submit']);

const localDish = ref({ ...props.dish });

watch(() => props.dish, (newVal) => {
  localDish.value = { ...newVal };
});

const submit = () => {
  emit('submit', localDish.value);
};
</script>
