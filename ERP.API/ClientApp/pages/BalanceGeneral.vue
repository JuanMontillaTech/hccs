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
    <div class="container" id="report">
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
                  <span style="padding-left: 130px" class="border col-lg-4">{{
                    subAccount.credit
                  }}</span>
                </div>
                <!-- <div style="padding-left: 310px">
                  </div> -->
              </div>
            </div>
          </div>
        </span>
      </div>
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
        .get("https://localhost:44367/api/Journal/CheckingBalance", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.dataReport = response.data.data;
          console.log(response.data.data);
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    printReport() {
      var mywindow = window.open("", "PRINT", "height=400,width=600");

      mywindow.document.write(
        "<html><head><title>" + document.title + "</title>"
      );
      mywindow.document.write("</head><body >");
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

<style></style>
