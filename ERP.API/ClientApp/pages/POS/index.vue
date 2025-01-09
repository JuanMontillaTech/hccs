<template>
  <div>
    <a href="/starter" class="btn btn-primary"po>Regresar</a>
     <POS></POS>
  </div>
</template>

<script>
 
export default {
  layout: "auth",
  data() {
    return {
      payables: [],
      
      totalRows: 0
    };
  },
  async mounted() {
     
  },
  methods: {
     
  }
};
</script>

<style>
/* Estilos opcionales */
</style>
