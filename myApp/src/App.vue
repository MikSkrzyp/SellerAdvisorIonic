<template>
  <ion-app>
    <ion-router-outlet />
  </ion-app>
</template>



<script setup lang="ts">
import { onMounted } from 'vue';
import { IonApp, IonRouterOutlet } from '@ionic/vue';
import { useFcmService } from '@/services/pushNotifications'; // Adjust path as per your project structure
import { useStorageService } from '@/services/storage.service'; // Adjust path as per your project structure

const { initPush } = useFcmService();
const { getToken } = useStorageService();

onMounted(() => {
  initPush();
  getToken().subscribe(token => {
    console.log('Received token:', token);
    // Handle token if needed
  }, error => {
    console.error('Error fetching token:', error);
  });
});
</script>

<style scoped>
/* Scoped styles for App.vue */
</style>
