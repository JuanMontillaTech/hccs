<template>
  <div class="container">
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <div>Listado de Reportes</div>
        </div>
      </div>
    </nav>
    <div class="container">
      <div>
        <div v-for="report in rows" :key="report.id">
          <span>
            <nuxt-link :to="report.link">
              <fa icon="file-lines" class="ml-1"></fa>
              <span class="menu-text">{{ report.name }}</span>
            </nuxt-link>
          </span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  layout: "TheSlidebar",
  name: "ReportesContables",
  data() {
    return {
      rows: [
        { id: 0, name: "Semestres 1", link: "/PrimerSemestre" },
        { id: 1, name: "Semestres 2", link: "" },
        { id: 2, name: "Estado de resultados", link: "" },
        { id: 3, name: "Balance general", link: "/BalanceGeneral" },
        { id: 4, name: "Libro diario", link: "" },
        { id: 5, name: "Auxiliar por tercero", link: "" },
        { id: 6, name: "Libro mayor", link: "" },
        { id: 7, name: "Movimientos por cuenta contable", link: "" },
        { id: 8, name: "Balanza de comprobación", link: "" },
        { id: 9, name: "Impuestos y retenciones", link: "" },
      ],
    };
  },
middleware: "authentication",

};
</script>

<style></style>
