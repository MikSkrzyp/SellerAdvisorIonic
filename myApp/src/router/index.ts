import { createRouter, createWebHistory } from '@ionic/vue-router';
import { RouteRecordRaw } from 'vue-router';
import Home from "@/views/Home.vue";
import PostItem from "@/views/PostItem.vue";
import EditItem from "@/views/EditItem.vue";
import Stats from "@/views/Stats.vue";


const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    component:Home
  },
  {
    path: '/postItem',
    component: PostItem
  },
  {
    path: '/edit-item/:id',
    name: 'EditItem',
    component: EditItem,
    props: true // Enable passing params as props
  },
  {
    path : '/stats',
    name: 'StatsView',
    component: Stats
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

export default router
