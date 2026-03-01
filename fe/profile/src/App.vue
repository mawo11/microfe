<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { RouterLink, RouterView } from 'vue-router'
import HelloWorld from './components/HelloWorld.vue'

const appTitle = 'Vue 3 app !!'

const navItemsJson = JSON.stringify([
  { label: 'Vue 3', href: 'https://vuejs.org/' }
]);

const layout = ref<HTMLElement | null>(null)

function handleAction(e: any) {
  alert('profile event!', e)
}

onMounted(() => {
  layout.value?.addEventListener('actionClick', handleAction)
})

onUnmounted(() => {
  layout.value?.removeEventListener('actionClick', handleAction)
})

</script>

<template>
  <uix-app-layout :menuTitle="appTitle" :menu-items="navItemsJson" ref="layout">

    <div style="padding: 2rem">
      <img alt="Vue logo" class="logo" src="@/assets/logo.svg" width="125" height="125" />

      <div class="wrapper">
        <HelloWorld msg="You did it!" />

        <nav>
          <RouterLink to="/">Home</RouterLink>
          <RouterLink to="/about">About</RouterLink>
        </nav>
      </div>
    </div>
    <div style="padding: 2rem">
      <RouterView />
    </div>

  </uix-app-layout>

</template>

<style scoped>
.logo {
  display: block;
  margin: 0 auto 2rem;
}

nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 2rem;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}


</style>
