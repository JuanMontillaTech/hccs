<template>
  <div class="container">
    <vue-good-table :columns="columns" :rows="rows" />
  </div>
</template>

<script>
export default {
  layout: "TheSlidebar",
  name: "ReportesContables",
  data() {
    return {
      columns: [
        {
          label: "Reportes contables",
          field: "name",
        },
      ],
      rows: [
        { id: 0, name: "Semestres 1" },
        { id: 1, name: "Semestres 2" },
        { id: 2, name: "Estado de resultados" },
        { id: 3, name: "Balance general" },
        { id: 4, name: "Libro diario" },
        { id: 5, name: "Auxiliar por tercero" },
        { id: 6, name: "Libro mayor" },
        { id: 7, name: "Movimientos por cuenta contable" },
        { id: 8, name: "Balanza de comprobación " },
        { id: 9, name: "Impuestos y retenciones" },
      ],
    };
  },
};
</script>

<style></style>
