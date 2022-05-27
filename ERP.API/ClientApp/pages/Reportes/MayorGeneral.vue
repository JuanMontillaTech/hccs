<template>
  <div>
    <PageHeader :title="title" :items="items" />    
    
    <div class="btn-group pb-2" role="group" aria-label="Basic example">
            <a @click="printReport()" class="btn btn-primary btn-sm text-white">
              <i class="uil-print me-2"></i>Imprimir
            </a>
            <a @click="downloadExcel()" class="btn btn-success btn-sm text-white">
              <i class="bx bx-spreadsheet me-2"></i>Excel
            </a>
          </div>

    <div v-if="itemsData.length > 0" id="report">

      <div class="row" style="display: flex;justify-content: space-around; margin-bottom: 20px;">

      <div class="col-lg-3" v-if="this.totales.totalDebit > 0">
        <b-card
          header-class="bg-transparent border-success"
          class="border border-success"
        >
          <template v-slot:header>
            <h5 class="my-0 text-success">
              <i class="uil-arrow-growth me-3"></i>Total Debito
            </h5>
          </template>
          <h3 class="">$ {{this.totales.totalDebit}}</h3>
        </b-card>
      </div>

      <div class="col-lg-3" v-else>
        <b-card
          header-class="bg-transparent border-danger"
          class="border border-danger"
        >
          <template v-slot:header>
            <h5 class="my-0 text-danger">
              <i class="mdi mdi-block-helper me-3"></i>Total Debito
            </h5>
          </template>
          <h3 class="">$ {{this.totales.totalDebit}}</h3>
        </b-card>
      </div>

      <div class="col-lg-3" v-if="this.totales.totalCredit > 0">
        <b-card
          header-class="bg-transparent border-success"
          class="border border-success"
        >
          <template v-slot:header>
            <h5 class="my-0 text-success">
              <i class="uil-arrow-growth me-3"></i>Total Credito
            </h5>
          </template>
          <h3 class="">$ {{this.totales.totalCredit}}</h3>
        </b-card>
      </div>

      <div class="col-lg-3" v-else>
        <b-card
          header-class="bg-transparent border-danger"
          class="border border-danger"
        >
          <template v-slot:header>
            <h5 class="my-0 text-danger">
              <i class="mdi mdi-block-helper me-3"></i>Total Credito
            </h5>
          </template>
          <h3 class="">$ {{this.totales.totalCredit}}</h3>
        </b-card>
      </div>

    </div>

    

      <div class="card">
        <div class="card-body">
          <div class="table-responsive">
            <table class="table table-striped mb-0">
              <thead>
                <tr>
                  <th v-for="item in headers">{{ item.label }}</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in itemsData">
                  <th scope="row">{{ item.name }}</th>
                  <td>{{ item.totalDebit }}</td>
                  <td>{{ item.totalCredit }}</td>
                  <td>{{ item.debtor }}</td>
                  <td>{{ item.creditor }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
    <div class="w-100 d-flex justify-content-center align-items-center snipper-h h-100" v-else>
      <b-spinner style="width: 3rem; height: 3rem" label="Large Spinner"></b-spinner>
    </div>
  </div>
</template>

<script>

export default {
  head() {
    return {
      title: `Reporte de ${this.title}`
    };
  },

  data() {
    return {
      name: "MayorGeneral",
      title: "Mayor General",
      items: [
        { text: "Reportes" },
        {
          text: "Mayor General",
          active: true
        }
      ],
      totales: {}, //debito, credito, deudor, acreedor
      headers: [
        { key: "name", label: "Cuenta" },
        { key: "totalDebit", label: "Débito" },
        { key: "totalCredit", label: "Crédito" },
        { key: "debtor", label: "Deudor" },
        { key: "creditor", label: "Acreedor" },
      ],
      itemsData: [],
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
      let url = this.$store.state.URL + "Journal/MajorGeneral";
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          console.log(response.data.data);
          this.itemsData = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async getTotals() {
      let url = this.$store.state.URL + "Journal/Totals";
      this.$axios
        .get(url, {
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

<!-- <style>
.snipper-h {
  height: 70vh;
}
</style> -->
