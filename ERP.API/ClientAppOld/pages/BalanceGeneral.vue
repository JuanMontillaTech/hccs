<template>
  <div class="container">
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <div>Reporte de Balance General</div>
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
      <b-table striped hover :items="items" :fields="fields">
        <template #cell(majorGeneralDetalls)="data">
          <div
            v-for="subAccount in data.item.majorGeneralDetalls"
            :key="subAccount.id"
          >
            {{ subAccount.code }}
            <div></div>
          </div>
        </template>
        <template #cell(debit)="data">
          <div
            v-for="subAccount in data.item.majorGeneralDetalls"
            :key="subAccount.id"
          >
            {{ subAccount.debit }}
          </div>
        </template>
        <template #cell(credit)="data">
          <div
            v-for="subAccount in data.item.majorGeneralDetalls"
            :key="subAccount.id"
          >
            {{ subAccount.credit }}
          </div>
        </template>
        <!-- <template #cell(totalDebit)="data">
          <div
            v-for="subAccount in data.item.majorGeneralDetalls"
            :key="subAccount.id"
          >
            {{ subAccount.credit }}
            <div></div>
          </div>
        </template>
        <template #cell(totalCredit)="data">
          <div
            v-for="subAccount in data.item.majorGeneralDetalls"
            :key="subAccount.id"
          >
            {{ subAccount.credit }}
            <div></div>
          </div>
        </template> -->
      </b-table>
    </div>
    <!-- <div class="container" id="report" v-if="dataReport.length > 0">
      <div class="row bg-primary text-white">
        <span class="col-lg-3 border py-2">Cuenta</span>
        <span class="col-lg-3 border py-2" style="padding-left: 30px"
          >Codigo</span
        >
        <span class="col-lg-3 border py-2" style="padding-left: 40px"
          >Debito</span
        >
        <span class="col-lg-3 border py-2" style="padding-left: 60px"
          >Credito</span
        >
      </div>
      <div v-for="account in dataReport" :key="account.id">
        <span>
          {{ account.name }}
          <div v-if="account.majorGeneralDetalls.length > 0">
            <div
              v-for="subAccount in account.majorGeneralDetalls"
              :key="subAccount.id"
            >
              <div style="padding-left: 100px">
                <div class="row">
                  <span style="padding-left: 180px" class="border col-lg-4">{{
                    subAccount.code
                  }}</span>
                  <span style="padding-left: 150px" class="border col-lg-4">{{
                    subAccount.debit
                  }}</span>
                  <span style="padding-left: 130px" class="border col-lg-4"
                    >{{ subAccount.credit }}
                  </span>
                </div>
              </div>
            </div>
            <div style="padding-left: 100px">
              <div class="row bg-secondary text-white">
                <span
                  style="padding-left: 180px; font-weight: bold"
                  class="border col-lg-4"
                  >Total</span
                >
                <span style="padding-left: 150px" class="border col-lg-4">
                  {{ account.totalDebit }}
                </span>
                <span style="padding-left: 130px" class="border col-lg-4">
                  {{ account.totalCredit }}
                </span>
              </div>
            </div>
          </div>
        </span>
      </div>
    </div> -->
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
      fields: [
        { key: "name", label: "Cuenta" },
        { key: "majorGeneralDetalls", label: "Código" },
        { key: "debit", label: "Débito" },
        { key: "credit", label: "Crédito" },
        { key: "totalDebit", label: "Total Crédito", variant: "primary" },
        { key: "totalCredit", label: "Total Débito", variant: "info" },
      ],
      // fields: ["items", "Código", "Débito", "Crédito"],
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
