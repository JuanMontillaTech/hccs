<template>
  <div class="container">
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <div>Reporte de Balance de Comprobacion</div>
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

    <div v-if="dataReport.length > 0" id="report">

    <div style="width: 100%;display: flex;justify-content: space-around; margin-bottom: 20px;">

      <div class="card card-default panel-primary" style="width: 20%;">
        <div class="card-header">Total Debito</div>
        <div class="card-body">
          <h4>$ {{this.totales.totalDebit}}</h4>
        </div>
      </div>

    <div class="card card-default" style="width: 20%;">
      <div class="card-header">Total Credito</div>
      <div class="card-body">
        <h4>$ {{this.totales.totalCredit}}</h4>
      </div>
    </div>

    <div class="card card-default" style="width: 20%;">
        <div class="card-header">Total Deudor</div>
        <div class="card-body card-5-6">
          <h4>$ {{this.totales.totalDebtor}}</h4>
        </div>
    </div>

    <div class="card card-default" style="width: 20%;">
      <div class="card-header">Total Acreedor</div>
      <div class="card-body card-5-6">
        <h4>$ {{this.totales.totalCreditor}}</h4>
      </div>
    </div>  
      
    </div>

    
      <b-table striped hover :items="items" :fields="fields">
      </b-table>
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
  name: "ReporteBalanceGeneral",
  data() {
    return {
      dataReport: [],
      totales: {}, //debito, credito, deudor, acreedor
      fields: [
        { key: "name", label: "Cuenta" },
        { key: "totalDebit", label: "Débito" },
        { key: "totalCredit", label: "Crédito" },
        { key: "debtor", label: "Deudor" },
        { key: "creditor", label: "Acreedor" },
      ],
      items: [],
      izitoastConfig: {
        position: "topRight",
      },
    };
  },
  created() {
    this.getAll();
    this.getTotals();
  },
  methods: {
    async getAll() {
      this.$axios
        .get(process.env.devUrl + "Journal/MajorGeneral", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          console.log(response.data.data);
          this.dataReport = response.data.data;
          this.items = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async getTotals() {
      this.$axios
        .get(process.env.devUrl + "Journal/Totals", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.totales = response.data.data;
          console.log(response.data.data);
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    // printReport() {
    //   var mywindow = window.open("", "PRINT", "height=800,width=1200");

    //   mywindow.document.write(
    //     "<html><head><title>" + document.title + "</title>"
    //   );
    //   mywindow.document.write("</head><body >");
    //   mywindow.document.write(
    //     '<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3" crossorigin="anonymous">'
    //   );
    //   mywindow.document.write("<h1>" + document.title + "</h1>");
    //   mywindow.document.write(document.getElementById("report").innerHTML);
    //   mywindow.document.write("</body></html>");

    //   mywindow.document.close(); // necessary for IE >= 10
    //   mywindow.focus(); // necessary for IE >= 10*/

    //   mywindow.print();
    //   mywindow.close();

    //   return true;
    // },
  },
};
</script>

<style>
.snipper-h {
  height: 70vh;
}
</style>
