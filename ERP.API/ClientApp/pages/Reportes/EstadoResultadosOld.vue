<template>
  <div class="container">
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <div>Reporte de Estado de Resultados</div>
        </div>
        <div class="btn-group" role="group" aria-label="Basic example">
          <a @click="printReport()" class="btn btn-primary btn-sm text-white">
            <fa icon="print" class="ml-1"></fa>
            Imprimir</a
          >

          <a @click="downloadExcel()" class="btn btn-success btn-sm text-white"
            ><fa icon="print" class="ml-1"></fa> Excel</a
          >
        </div>
      </div>
    </nav>

    <div v-if="items.length > 0" id="report">

    <b-table striped hover :items="items" :fields="fields">
    </b-table>

    <hr style="margin-top: 10px;margin-bottom: 10px;color: black; height: 8px;">
    
    <div style="display: flex;justify-content: space-around;font-size: 1.5rem;line-height: 2rem;">
      <span style="">Total:</span>
      <span style="color: red;" v-if="Total < 0">
        $ {{ Total }}
      </span>
      <span style="color: green;" v-else>
        $ {{ Total }}
      </span>
    </div>
    </div>
    <div
      class="w-100 d-flex justify-content-center align-items-center snipper-h"
      v-else
    >
      <b-spinner
        style="width: 3rem; height: 3rem"
        label="Large Spinner"
      ></b-spinner>
    </div>
  </div>
</template>

<script>
export default {
  layout: "TheSlidebar",
  name: "ReporteEstadoResultados",
  data() {
    return {
      Total: 0,
      fields: [
        { key: "type", label: "Tipo" },
        { key: "name", label: "Nombre" },
        { key: "total", label: "Monto" },
      ],
      items: [],
      izitoastConfig: {
        position: "topRight",
      },
    };
  },
  created() {
    this.getAll();
  },
  methods: {
    async getAll() {
      this.$axios
        .get(   "Journal/StatementIncome", {
          headers: {
            "Content-Type": "application/json",
             Authorization: `${localStorage.getItem("authUser")}`,
          },
        })
        .then((response) => {
          this.Total = response.data.data.amount;
          this.items = response.data.data.statementIncomes;
          console.log(response.data.data);
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
