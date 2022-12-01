<template>
  <div>
 
    <h4>{{ this.DataForm.title }}</h4>
    <div class="row">
      <div class="col-lg-12">
        <div
          v-for="(SectionRow, SectionIndex) in DataFormSection"
          :key="SectionIndex"
        >
          <div class="row ml-0 mb-12">
            <div class="col-lg-12 col-md-12 col-sm-12">
              <span style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ SectionRow.name }}</span>
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
        <b-button-group>
          <b-button variant="success" size="lg" @click="saveSchema()">
            <i class="bx bx-save"></i> Crear
          </b-button>
        </b-button-group>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: ["FormIdExt"],
  data() {
    return {
      FormId: "",
      RowId: "",
      DataForm: [],
      DataFormSection: [],
      DataFormSectionGrids: [],
      principalSchema: {},
      SchemaTable: [],
    };
  },
  watch: {
    FormIdExt() {
      this.FormId = "";
      this.RowId = "";
      this.DataForm = [];
      this.DataFormSection = [];
      this.DataFormSectionGrids = [];
      this.principalSchema = {};
      this.SchemaTable = [];
      this.FormId = this.FormIdExt;

      this.GetFormRows();
    },
  },
  middleware: "authentication",

  mounted() {
    this.GetFormRows();
  },
  methods: {
    removeRow(index) {
      this.DataFormSectionGrids.splice(index, 1);
    },
    addRow() {
      //   let newrow = {};
      //   this.DataFormSectionGrids[index].fields.map((schema) => {
      //  newrow =schema;
      //   });
      //     console.log(newrow);
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
    clearForm() {
      for (const key in this.principalSchema) {
        this.principalSchema[key] = "";
      }
    },
    GetFormRows() {
      this.FormId = this.FormIdExt;
      var url = `Form/GetById?Id=${this.FormIdExt}`;
      this.DataForm = [];
      this.DataFormSection = [];
      this.DataFormSectionGrids = [];

      this.$axios
        .get(url)
        .then((response) => {
          this.DataForm = response.data.data;

          this.GetFilds();
          if (this.$route.query.Action === "edit") {
            this.RowId = this.$route.query.Id;
            this.GetFildsData();
          }
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
    GetGrids: function () {
      this.$axios
        .get(`FormGrid/GetSectionWithFildsByFormID/${this.FormId}`)
        .then((response) => {
          this.DataFormSectionGrids = response.data.data;
          this.SchemaTable.push(newrow);
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },

    GetFildsData() {
      var url = `${this.DataForm.controller}/GetById?Id=${this.RowId}`;
      this.$axios
        .get(url)
        .then((response) => {
          this.principalSchema = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GoBack() {
      if (this.DataForm.backList) {
        this.$router.push({ path: `/ExpressForm/Index?Form=${this.FormId}` });
      }
    },
    editSchema() {
      if (this.DataForm.edit == true) {
        this.put();
      } else {
        this.$toast.info("La opcion editar esta deshabilitada");
      }
    },
    saveSchema() {
      if (this.RowId.length < 1) {
        if (this.DataForm.create == true) {
          this.post();
        } else {
          this.$toast.info("La opcion crear esta deshabilitada");
        }
      }
    },

    post() {
      let url = `${this.DataForm.controller}/Create`;
      let result = null;

      this.$axios
        .post(url, this.principalSchema)
        .then((response) => {
          console.log(response);
          result = response;
          this.$toast.success(
            "El Registro ha sido creado correctamente.",
            "ÉXITO"
          );
          // this.GoBack();
        })
        .catch((error) => {
          result = error;
          this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
        });
    },
    put() {
      this.$axios
        .put(`${this.DataForm.controller}/Update`, this.principalSchema)
        .then((response) => {
          this.$toast.success(
            "El Registro ha sido actualizado correctamente.",
            "EXITO"
          );

          this.GoBack();
        })
        .catch((error) => {
          reject(error);
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
  },
};
</script>

<style>
/* Red border */
hr.new1 {
  border-top: 1px solid blue;
}

.text-size-required {
  font-size: 12px;
}
</style>
