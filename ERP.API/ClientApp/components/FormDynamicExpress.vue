<template>
  <div>
    <h4>{{ this.DataForm.title }}</h4>
    {{ count }}
    <div class="row">
      <div class="col-lg-12">
        <div class="card">
          <div class="card-body">
            <div class="container">
              <div class="row">
                <div v-for="(item, index) in fields" :key="index" class="col-6">
                  <div v-if="item.columnIndex == 1">
                    <div class="row">
                      <div class="col-12">
                        <b-form-group :label="item.label" v-if="item.type == 0">
                          <b-form-input
                            v-model="principalSchema[item.field]"
                            size="sm"
                            trim
                          >
                          </b-form-input>
                        </b-form-group>
                        <b-form-group v-if="item.type == 5">
                          <b-form-textarea
                            :label="item.label"
                            v-model="principalSchema[item.field]"
                            rows="3"
                            max-rows="6"
                          ></b-form-textarea>
                        </b-form-group>
                        <b-form-group :label="item.label" v-if="item.type == 1">
                          <!-- <li v-for="item in items">
                            {{ item.title }}
                          </li> -->
                          <!-- :options="list" -->
                          {{ getList(item.sourceApi) }}

                          <vueselect
                            v-model="principalSchema[item.field]"
                            :options="getList(item.sourceApi)"
                            placeholder="Seleccione"
                            :reduce="(row) => row.id"
                            label="title"
                            size="sm"
                          >
                          </vueselect>
                        </b-form-group>

                        <b-form-group :label="item.label" v-if="item.type == 3">
                          <b-form-checkbox
                            v-model="principalSchema[item.field]"
                            name="checkbox-1"
                            value="true"
                            unchecked-value="false"
                          >
                            {{ item.label }}
                          </b-form-checkbox>
                        </b-form-group>
                      </div>
                    </div>
                  </div>
                </div>
                <div v-for="(item, index) in fields" :key="index" class="col-6">
                  <div v-if="item.columnIndex == 2">
                    <div class="row">
                      <div class="col-12">
                        <b-form-group :label="item.label" v-if="item.type == 0">
                          <b-form-input
                            v-model="principalSchema[item.field]"
                            size="sm"
                            trim
                          >
                          </b-form-input>
                        </b-form-group>
                        <b-form-group :label="item.label" v-if="item.type == 3">
                          <b-form-checkbox
                            v-model="principalSchema[item.field]"
                            name="checkbox-1"
                            value="accepted"
                            unchecked-value="not_accepted"
                          >
                            {{ item.label }}
                          </b-form-checkbox>
                        </b-form-group>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <div class="row justify-content-end w-100 gx-2">
              <div class="col-3 p-2" v-if="$route.query.Action == 'edit'">
                <b-button-group class="mt-4 mt-md-0">
                  <b-button variant="secundary" class="btn" @click="GoBack()">
                    <i class="bx bx-arrow-back"></i> Regresar
                  </b-button>
                  <b-button variant="success" class="btn" @click="editSchema()">
                    <i class="bx bx-save"></i> Guardar
                  </b-button>
                </b-button-group>
              </div>
              <div class="col-3 p-2" v-else>
                <b-button-group class="mt-4 mt-md-0">
                  <b-button variant="secundary" class="btn" @click="GoBack()">
                    <i class="bx bx-arrow-back"></i> Regresar
                  </b-button>
                  <b-button variant="success" size="lg" @click="saveSchema()">
                    <i class="bx bx-save"></i> Guardar
                  </b-button>
                </b-button-group>
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
    </div>
  </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
export default {
  head() {
    return {
      title: `${this.DataForm.title} | ERP Cardenal Sancha`,
    };
  },
  data() {
    return {
      count: 0,
      FormId: "",
      dataList: [],
      RowId: "",
      DataForm: [],
      fields: [],
      principalSchema: {},
      listOfElements: [],
      items: [],
      list: [
        { id: 1, title: "test 1" },
        { id: 2, title: "test 2" },
        { id: 3, title: "test 3" },
      ],
    };
  },

  middleware: "authentication",

  created() {
    this.GetFormRows();
  },
  methods: {
    getList(UrlApi) {
      let arr = [];
      let url = this.$store.state.URL + `${UrlApi}/GetAll`;
      fetch(url, {
        method: "GET",
        headers: {
          "Content-type": "application/json;charset=UTF-8",
          Authorization: `${localStorage.getItem("authUser")}`,
        },
      })
        .then((response) => response.json())
        .then((data) => {
          arr = data.data;
        });
      // let respDataStr = arr;
      // let jsObject = JSON.parse(respDataStr);

      return { arr};
    },
    clearForm() {
      for (const key in this.principalSchema) {
        this.principalSchema[key] = "";
      }
    },
    GetFormRows() {
      this.FormId = this.$route.query.Form;
      this.DataForm = [];
      this.$axios
        .get(`Form/GetById/${this.$route.query.Form}`)
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
    GetFilds() {
      this.$axios
        .get(`Formfields/GetByFormId/${this.FormId}`)
        .then((response) => {
          //  response.data.data.map((row) => {

          //   if(row.type === 1)

          //   this.getList(row.sourceApi,row.id )
          // });
          this.fields = response.data.data;
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
      this.$router.push({ path: `/ExpressForm/Index?Form=${this.FormId}` });
    },
    editSchema() {
      this.put(this.principalSchema);
    },
    saveSchema() {
      if (this.RowId.length < 1) {
        this.post();
      } else {
        this.put();
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
