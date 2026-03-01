// stores/dashboard.store.ts
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { DiagItem } from './models'

export const useProfileStore = defineStore('Porfile', () => {
  // state
  const items = ref<DiagItem[]>([])
  const loading = ref(false)

  // computed
  const getDiagItems = computed(() => items.value)

  // actions
  async function loadDiagItems() {
    console.log('fetching diag items')
    loading.value = true

    try {
      const response = await fetch('/api/profile/diagnostic')
      const data: DiagItem[] = await response.json()
      console.log('fetched diag items', data)
      items.value = data
    } catch (error) {
      console.error('failed to fetch diag items', error)
    } finally {
      loading.value = false
    }
  }

  return { items, loading, getDiagItems, loadDiagItems }
})