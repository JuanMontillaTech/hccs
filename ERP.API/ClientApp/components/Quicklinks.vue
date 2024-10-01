<template>
    <div class="quick-links">
        <h6>Accesos directos</h6>
      <ul class="list-group">
        <li v-for="link in sortedLinks" :key="link.index" class="list-group-item">
          <a 
            :class="{ 
              'list-group-item': true, 
              [`list-group-item-${link.type}`]: true, // Clase dinámica para el tipo
              'active': link.isActive 
            }" 
            href="#" 
            @click.prevent="handleClick(link)"
          >
            {{ link.title }}
          </a>
        </li>
      </ul>
    </div>
  </template>
  
  <script>
  export default {
    name: 'QuickLinks',
    props: {
      links: {
        type: Array,
        required: true,
        validator(value) {
          return value.every(link => {
            return (
              typeof link.title === 'string' &&
              typeof link.index === 'number' &&
              typeof link.isActive === 'boolean' &&
              ['primary', 'secondary', 'success', 'danger', 'warning', 'info', 'light', 'dark', ''].includes(link.type)
            );
          });
        }
      }
    },
    computed: {
      sortedLinks() {
        return [...this.links].sort((a, b) => a.index - b.index);
      }
    },
    methods: {
      handleClick(link) {
           
      this.$router.push({
        path: link.links,
      });
      }
    }
  };
  </script>