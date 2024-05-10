<template>
  <div>
    <div class="card-body">
      <div class="d-print-none mt-4">
        <div class="card">
          <div class="card-body">
            <h4>{{ this.DataForm.title }}</h4>
            <div
              v-for="(SectionRow, SectionIndex) in DataFormSection"
              :key="SectionIndex"
            >
              <div class="row ml-0 mb-3">
                <div class="col-lg-12 col-md-12 col-sm-12">
                  <h4>{{ SectionRow.name }}</h4>
                  <hr class="new1" />
                </div>
              </div>

              <div class="row">
                <div
                  v-for="(fieldsRow, fieldIndex) in GetFilterDataOnlyshowForm(
                    SectionRow.fields
                  )"
                  :key="fieldIndex"
                >
                  <DynamicElementGrid
                    @CustomChange="GetValueFormElement"
                    :FieldsData="principalSchema"
                    :item="fieldsRow"
                    :labelShow="true"
                  ></DynamicElementGrid>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-md-4 float-end">
                <b-button
                  variant="primary"
                  class="btn mt-4 btn-success waves-effect waves-light"
                  @click="getReport()"
                >
                  Buscar
                </b-button>
                <a
                  v-if="ReportData.length"
                  href="javascript:window.print()"
                  class="btn btn-success waves-effect waves-light mt-4"
                >
                  <i class="fa fa-print"></i>
                </a>
                <button
                  v-if="ReportData.length"
                  class="btn mt-4 btn-success waves-effect waves-light"
                  @click="exportarExcel"
                >
                  <i class="fa fa-file-excel"></i>
                </button>
              </div>
            </div>

            <div v-if="ReportData.length" class="row">
              <div class="col-md-4">
                <b-alert variant="success" class="btn mt-4" show>
                  cantidad registros encontrados {{ ReportData.length }}
                </b-alert>
              </div>
            </div>

            <div v-else class="row">
              <div class="col-md-4">
                <b-alert variant="success" class="btn mt-4" show>
                  cantidad registros encontrados {{ ReportData.length }}
                </b-alert>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div
        class="ticket"
        style="font: 14px Lucida Console; background-color: white; color: black"
      >
        <b-table
          ref="miTabla"
          outlined
          class="table table-nowrap table-centered mb-0"
          striped
          small
          Bordered
          responsive
          Hover
          :items="ReportData"
          :fields="fields"
        >
        </b-table>
      </div>
      <div class="d-print-none mt-4">
        <div class="float-end">
          <a
            href="javascript:window.print()"
            class="btn btn-success waves-effect waves-light mr-1"
          >
            <i class="fa fa-print"></i>
          </a>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
var numbro = require("numbro");
var moment = require("moment");
import * as XLSX from "xlsx"; // Importar XLSX
export default {
  head() {
    return {
      title: `Reporte ${this.DataForm.title}`,
    };
  },
  data() {
    return {
      FormId: "",
      Ticket: [],
      ReportData: [],
      FileName: "Reporte",
      PageCreate: "/ExpressForm/FuncionalFormExpress",
      DataForm: [],
      DataFormSection: [],
      DataFormSectionGrids: [],
      principalSchema: {},
    };
  },
  watch: {
    "$route.query.Form"() {
      this.GetForm();
    },
  },
  //middleware: "authentication",

  created() {
    this.FormId = this.$route.query.Form;
    this.GetForm();
  },
  methods: {
    exportarExcel() {
      // Obtener el componente b-table a través de la referencia y luego acceder al elemento DOM
      const table = this.$refs.miTabla.$el;
      this.FileName = `Reporte ${this.DataForm.title}`.replace(/\s/g, "_");

      // Crear una hoja de trabajo a partir del elemento DOM de la tabla
      const worksheet = XLSX.utils.table_to_sheet(table);

      // Crear un libro de trabajo y agregar la hoja de trabajo
      const workbook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(workbook, worksheet, "Reporte");

      // Generar el archivo Excel
      XLSX.writeFile(workbook, this.FileName + ".xlsx");
    },
    GetForm() {
      var url = `Form/GetById?Id=${this.FormId}`;
      this.DataForm = [];
      this.DataFormSection = [];
      this.DataFormSectionGrids = [];

      this.$axios
        .get(url)
        .then((response) => {
          this.DataForm = response.data.data;

          this.GetFilds();
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },

    GoBack() {
      this.$router.push({ path: `/ExpressForm/Index?Form=${this.Form}` });
    },
    GoNew() {
      this.$router.push({
        path: `${this.PageCreate}`,
        query: {
          Form: this.Form,
          Action: "create",
          id: null,
        },
      });
    },
    GetFilterDataOnlyshowForm(fields) {
      let results = fields.filter((rows) => rows.showForm == 1);
      return results;
    },
    GetValueFormElement(formElemen) {
      this.principalSchema = formElemen;
    },
    GetLitValue(filds, Value) {
      this.principalSchema[filds] = Value;
    },
    async getReport() {
      let data = JSON.stringify(this.principalSchema);
      let url = `Report/GetById?id=${this.FormId}&Data=${data}`;

      console.log(url);

      this.$axios
        .get(url)
        .then((response) => {
          this.ReportData = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetFilds: function () {
      this.$axios
        .get(`Formfields/GetSectionWithFildsByFormID/${this.FormId}`)
        .then((response) => {
          this.DataFormSection = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
  },
};
</script>

<style></style>
