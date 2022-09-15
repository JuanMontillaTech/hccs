<template>
  <div>
    <div
      class="alert alert-light"
      role="alert"
      v-html="DataForm.commentary"
      v-if="DataForm.commentary"
    ></div>
    <h4>{{ this.DataForm.title }}</h4>
    <div class="row">
      <div class="col-lg-12">
        <div class="alert alert-light" role="alert">
          <div v-if="$route.query.Action == 'edit'">
            <b-button-group>
              <b-button
                variant="secundary"
                class="btn"
                @click="GoBack()"
                v-if="DataForm.backList"
              >
                <i class="bx bx-arrow-back"></i> Regresar
              </b-button>
              <b-button variant="success" class="btn" @click="editSchema()">
                <i class="bx bx-save"></i> Actualizar
              </b-button>
            </b-button-group>
          </div>
          <div v-else>
            <b-button-group>
              <b-button variant="secundary" class="btn" @click="GoBack()">
                <i class="bx bx-arrow-back"></i> Regresar
              </b-button>
              <b-button variant="success" size="lg" @click="saveSchema()">
                <i class="bx bx-save"></i> Crear
              </b-button>
            </b-button-group>
          </div>
        </div>
        <div class="card">
          <div class="card-body">
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
    
           

            <div class="row ml-0 mb-3">
              <div class="col-lg-12 col-md-12 col-sm-12">
                <hr class="new1" />
                <b-form-group id="input-group-2" label-for="input-2">
                  <h4 class="card-title">Comentario</h4>
                  <b-form-textarea
                    id="textarea"
                    v-model="principalSchema.commentary"
                    rows="3"
                    max-rows="6"
                  >
                  </b-form-textarea>
                </b-form-group>
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-12 col-md-12 col-sm-12">
                <h4>Auditoría</h4>
                <hr class="new1" />
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group
                  id="input-group-2"
                  label="Creado Por :"
                  label-for="input-2"
                >
                  <b-form-input
                    id="textarea"
                    v-model="principalSchema.createdBy"
                    rows="3"
                    disabled
                    max-rows="6"
                  >
                  </b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group
                  id="input-group-2"
                  label="Creado en :"
                  label-for="input-2"
                >
                  <b-form-input
                    id="textarea"
                    v-model="principalSchema.createdDate"
                    rows="3"
                    disabled
                    max-rows="6"
                  >
                  </b-form-input>
                </b-form-group>
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group
                  id="input-group-2"
                  label="Modificado Por:"
                  label-for="input-2"
                >
                  <b-form-input
                    id="textarea"
                    v-model="principalSchema.createdBy"
                    rows="3"
                    disabled
                    max-rows="6"
                  >
                  </b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group
                  id="input-group-2"
                  label="Modificado en:"
                  label-for="input-2"
                >
                  <b-form-input
                    id="textarea"
                    v-model="principalSchema.lastModifiedDate"
                    rows="3"
                    disabled
                    max-rows="6"
                  >
                  </b-form-input>
                </b-form-group>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="alert alert-light" role="alert">
        <div v-if="$route.query.Action == 'edit'">
          <b-button-group>
            <b-button
              variant="secundary"
              class="btn"
              @click="GoBack()"
              v-if="DataForm.backList"
            >
              <i class="bx bx-arrow-back"></i> Regresar
            </b-button>
            <b-button variant="success" class="btn" @click="editSchema()">
              <i class="bx bx-save"></i> Actualizar
            </b-button>
          </b-button-group>
        </div>
        <div v-else>
          <b-button-group>
            <b-button variant="secundary" class="btn" @click="GoBack()">
              <i class="bx bx-arrow-back"></i> Regresar
            </b-button>
            <b-button variant="success" size="lg" @click="saveSchema()">
              <i class="bx bx-save"></i> Crear
            </b-button>
          </b-button-group>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  head() {
    return {
      title: `${this.DataForm.title}`,
    };
  },
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
    "$route.query.Form"() {
      this.FormId = "";
      this.RowId = "";
      this.DataForm = [];
      this.DataFormSection = [];
      this.DataFormSectionGrids = [];
      this.principalSchema = {};
      this.SchemaTable = [];
      this.FormId = this.$route.query.Form;
   
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
      this.FormId = this.$route.query.Form;
      var url = `Form/GetById?Id=${this.$route.query.Form}`;
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
          result = response;
          this.$toast.success(
            "El Registro ha sido creado correctamente.",
            "ÉXITO"
          );
          this.GoBack();
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
