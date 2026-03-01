<script setup lang="ts">
import { onMounted } from 'vue'
import { useProfileStore } from '../stores/profile.store'

const store = useProfileStore()

onMounted(() => {
  store.loadDiagItems()
})
</script>

<template>
  <div class="about">
    <h1>This is an diagnostic page</h1>
     <div v-if="store.loading">Ładowanie...</div>

        <table v-else>
        <tr>
            <th>klucz</th>
            <th>wartosc</th>
        </tr>
        <tr v-for="item in store.getDiagItems" :key="item.key">
            <td>{{item.key}}</td>
            <td class="text">{{item.value}}</td>
        </tr>
    </table>
  </div>
</template>
<style scoped>

  table {
    margin: auto;
  }
  table td:first-child {
    font-weight: bold;
    text-align: right;
    padding-right: 10px;
  }
   table td:last-child {
      text-align: left;
    padding-left: 10px;
        white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    max-width: 100ch;
  }
</style>
