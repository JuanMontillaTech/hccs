<template>
  <div style="min-height: 100vh;">
    <PageHeader :title="title" :items="items" />

    <div class="btn-group pb-2" role="group" aria-label="Basic example">
      <a @click="printReport()" class="btn btn-primary btn-sm text-white">
        <i class="uil-print me-2"></i>Imprimir
      </a>
      <a @click="downloadExcel()" class="btn btn-success btn-sm text-white">
        <i class="bx bx-spreadsheet me-2"></i>Excel
      </a>
    </div>

    <div class="col-sm-8">
        <label>Opciones de Filtrado</label>
        <multiselect
          @input="getAll()"
          v-model="value"
          :options="options"
          :allow-empty="false"
          class="helo"
        ></multiselect>
      </div>

    <div v-if="itemsData.length > 0" id="report">
      <div class="mt-4 m-4">
        <div v-for="item in itemsData" class="p-4 d-flex justify-content-between">
          <div>
            <p class="fs-6">{{ item.type }}</p>
            <p class="fs-5">{{ item.name }}</p>
          </div>
          <span class="fs-4 me-4 fw-bold">{{ item.total }}</span>
        </div>
      </div>

      <hr class="bg-dark d-flex justify-content-between m-3" style="height: 5px;" />
      <div>
        <p class="fs-3">Total: </p>
      <p class="fs-3">{{ Total }}</p>
      </div>
    </div>
    <div class="w-100 d-flex justify-content-center align-items-center snipper-h h-100" v-else>
      <b-spinner style="width: 3rem; height: 3rem" label="Large Spinner"></b-spinner>
    </div>
  </div>
</template>

<script>
import Multiselect from "vue-multiselect";

import "vue2-datepicker/index.css";
import "vue-multiselect/dist/vue-multiselect.min.css";

export default {
  name: "ReporteEstadoResultados",
  head() {
    return {
      title: `Reporte de ${this.title}`
    };
  },
  components: {
    Multiselect,
  },
  data() {
    return {
      title: "Estado de Resultados",
      items: [
        { text: "Reportes" },
        {
          text: "Estado de Resultados",
          active: true
        }
      ],
      value: null,
      options: ["Cargando...."],
      Total: 0,
      fields: ["Tipo", "Nombre", "Monto"],
      itemsData: [],
      izitoastConfig: {
        position: "topRight",
      },
    };
  },
  created() {
    this.getOptions();
  },
  methods: {
    async getAll() {
      let url = this.$store.state.URL + "Journal/StatementIncome/" + this.value;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          console.log(response.data.data);
          this.Total = response.data.data.amount;
          this.itemsData = response.data.data.statementIncomes;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async getOptions() {
      let url = this.$store.state.URL + "Journal/StatementIncomeOptions";
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          console.log(response.data.data);
          this.options = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    printReport() {
      var mywindow = window.open("", "PRINT", "height=800,width=1200");

      mywindow.document.write(
        "<html><head><title>" + document.title + "</title>"
      );
      mywindow.document.write("</head><body >");
      mywindow.document.write(
        '<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3" crossorigin="anonymous">'
      );
      mywindow.document.write("<h1>" + document.title + "</h1>");
      mywindow.document.write(document.getElementById("report").innerHTML);
      mywindow.document.write("</body></html>");

      mywindow.document.close(); // necessary for IE >= 10
      mywindow.focus(); // necessary for IE >= 10*/

      mywindow.print();
      mywindow.close();

      return true;
    },
  },
};
</script>

<style>
.snipper-h {
  height: 70vh;
}
</style>
