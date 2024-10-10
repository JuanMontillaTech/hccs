<template>
    <div>
      <h4>{{ DataForm.title }}</h4>
      <ValidationObserver ref="form">
        <form @submit.prevent="onSubmit"> 
          <div class="row">
            <div class="col-3 p-2">
              <b-button-group class="mt-4 mt-md-0">
                <b-button variant="secondary" class="btn" size="sm" @click="GoBack()">
                  <i class="bx bx-arrow-back"></i> Lista
                </b-button>
                <b-button variant="success" title="Guardar" @click="onSubmit" size="sm">
                  <i class="uil uil-print font-size-18"></i> Guardar
                </b-button>
              </b-button-group>
            </div>
            <div class="col-lg-12 p-4">
              <div class="card">
                <div class="card-header" style="background-color: white">
                  <h5 class="text-center">{{ DataForm.title }}</h5>
                </div>
                <div class="card-body justify-content-center container">
                  <div class="row">
                    <div class="col-md-6">
                      <b-form-group label="Nombre" label-for="name" :label-cols="4">
                        <ValidationProvider rules="required" v-slot="{ errors }">
                          <b-form-input id="name" v-model="asset.Name" type="text" size="sm" :state="errors.length > 0 ? false : null" />
                          <span class="text-danger" v-if="errors[0]">{{ errors[0] }}</span>
                        </ValidationProvider>
                      </b-form-group>
                    </div>
                    <div class="col-md-6">
                      <b-form-group label="Clase de Activo" label-for="assetClassId" :label-cols="4">
                        <ValidationProvider rules="required" v-slot="{ errors }">
                          <b-form-select id="assetClassId" v-model="asset.AssetClassId" :options="assetClasses" size="sm" :state="errors.length > 0 ? false : null" />
                          <span class="text-danger" v-if="errors[0]">{{ errors[0] }}</span>
                        </ValidationProvider>
                      </b-form-group>
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-md-6">
                      <b-form-group label="Ubicación" label-for="locationId" :label-cols="4">
                        <ValidationProvider rules="required" v-slot="{ errors }">
                          <b-form-select id="locationId" v-model="asset.LocationId" :options="locations" size="sm" :state="errors.length > 0 ? false : null" />
                          <span class="text-danger" v-if="errors[0]">{{ errors[0] }}</span>
                        </ValidationProvider>
                      </b-form-group>
                    </div>
                    <div class="col-md-6">
                      <b-form-group label="Fecha de Adquisición" label-for="acquisitionDate" :label-cols="4">
                        <ValidationProvider rules="required" v-slot="{ errors }">
                          <b-form-input id="acquisitionDate" v-model="asset.AcquisitionDate" type="date" size="sm" :state="errors.length > 0 ? false : null" />
                          <span class="text-danger" v-if="errors[0]">{{ errors[0] }}</span>
                        </ValidationProvider>
                      </b-form-group>
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-md-6">
                      <b-form-group label="Costo de Adquisición" label-for="acquisitionCost" :label-cols="4">
                        <ValidationProvider rules="required|numeric" v-slot="{ errors }">
                          <b-form-input id="acquisitionCost" v-model="asset.AcquisitionCost" type="number" size="sm" :state="errors.length > 0 ? false : null" />
                          <span class="text-danger" v-if="errors[0]">{{ errors[0] }}</span>
                        </ValidationProvider>
                      </b-form-group>
                    </div>
                    <div class="col-md-6">
                      <b-form-group label="Valor Residual" label-for="residualValue" :label-cols="4">
                        <b-form-input id="residualValue" v-model="asset.ResidualValue" type="number" size="sm" />
                      </b-form-group>
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-md-6">
                      <b-form-group label="Vida Útil" label-for="usefulLife" :label-cols="4">
                        <b-form-input id="usefulLife" v-model="asset.UsefulLife" type="number" size="sm" />
                      </b-form-group>
                    </div>
                    <div class="col-md-6">
                      <b-form-group label="Método de Depreciación" label-for="depreciationMethod" :label-cols="4">
                        <b-form-select id="depreciationMethod" v-model="asset.DepreciationMethod" :options="depreciationMethods" size="sm" />
                      </b-form-group>
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-md-6">
                      <b-form-group label="Número de Serie" label-for="serialNumber" :label-cols="4">
                        <b-form-input id="serialNumber" v-model="asset.SerialNumber" type="text" size="sm" />
                      </b-form-group>
                    </div>
                    <div class="col-md-6">
                      <b-form-group label="Marca" label-for="brand" :label-cols="4">
                        <b-form-input id="brand" v-model="asset.Brand" type="text" size="sm" />
                      </b-form-group>
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-md-6">
                      <b-form-group label="Modelo" label-for="model" :label-cols="4">
                        <b-form-input id="model" v-model="asset.Model" type="text" size="sm" />
                      </b-form-group>
                    </div>
                    <div class="col-md-6">
                      <b-form-group label="Estado" label-for="status" :label-cols="4">
                        <b-form-select id="status" v-model="asset.Status" :options="statusOptions" size="sm" />
                      </b-form-group>
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-md-12">
                      <b-form-group label="Comentario" label-for="commentary" :label-cols="2">
                        <b-form-textarea id="commentary" v-model="asset.Commentary" rows="3" size="sm" />
                      </b-form-group>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-3 p-2">
              <b-button-group class="mt-4 mt-md-0">
                <b-button variant="secondary" class="btn" size="sm" @click="GoBack()">
                  <i class="bx bx-arrow-back"></i> Lista
                </b-button>
                <b-button variant="success" title="Guardar" @click="onSubmit" size="sm">
                  <i class="uil uil-print font-size-18"></i> Guardar
                </b-button>
              </b-button-group>
            </div>
          </div>
        </form>
      </ValidationObserver>
    </div>
  </template>
  
  <script>
  import Swal from "sweetalert2";
  
  export default {
    data() {
      return {
        asset: {
          Id: null,
          Name: "",
          AssetClassId: null,
          LocationId: null,
          AcquisitionDate: null,
          AcquisitionCost: 0,
          ResidualValue: 0,
          UsefulLife: 0,
        DepreciationMethod: null,
        Status: null,
        SerialNumber: "",
        Brand: "",
        Model: "",
        Commentary: "",
      },
      assetClasses: [],
      locations: [],
      depreciationMethods: [
        { value: 'Lineal', text: 'Lineal' },
        { value: 'Regresivo', text: 'Regresivo' },
        { value: 'Unidades de Producción', text: 'Unidades de Producción' }
      ],
      statusOptions: [
        { value: 'En uso', text: 'En uso' },
        { value: 'En mantenimiento', text: 'En mantenimiento' },
        { value: 'Dado de baja', text: 'Dado de baja' }
      ],
      DataForm: [],
    };
  },
  async mounted() {
    await this.fetchAssetClasses();
    await this.fetchLocations();
    this.loadData();
  },
  methods: {
    async fetchAssetClasses() {
      try {
        const response = await this.$axios.get('AssetClass/GetAll');
        this.assetClasses = response.data.data.map(item => ({ value: item.id, text: item.name }));
      } catch (error) {
        this.$toast.error(error.message, "Error al obtener clases de activo", this.izitoastConfig);
      }
    },
    async fetchLocations() {
      try {
        const response = await this.$axios.get('Location/GetAll');
        this.locations = response.data.data.map(item => ({ value: item.id, text: item.name }));
      } catch (error) {
        this.$toast.error(error.message, "Error al obtener ubicaciones", this.izitoastConfig);
      }
    },
    GoBack() {
      this.$router.push("/asset"); 
    },
    async onSubmit() {
      try {
        const isValid = await this.$refs.form.validate();
        if (isValid) {
          if (this.asset.Id) {
            console.log(this.asset);
            await this.$axios.put('Asset/Update', this.asset);
            this.$toast.success("Activo actualizado correctamente.", "Éxito", this.izitoastConfig);
          } else {
            await this.$axios.post('Asset/Create', this.asset);
            this.$toast.success("Activo creado correctamente.", "Éxito", this.izitoastConfig);
          }
          this.GoBack();
        }
      } catch (error) {
        this.$toast.error(error.message, "Error al guardar el activo", this.izitoastConfig);
      }
    },
    loadData() {
      const id = this.$route.params.id;
      if (id) {
        this.fetchAsset(id);
      } else {
        this.asset.Id = null; // Asegurar que el ID es nulo para un nuevo activo
      }
    },
    async fetchAsset(id) {
      try {
        const response = await this.$axios.get(`Asset/GetById?id=${id}`);
        this.asset = response.data.data;
      } catch (error) {
        this.$toast.error(error.message, "Error al obtener el activo", this.izitoastConfig);
      }
    }
  }
};
</script>

<style>
  /* Estilos opcionales */
  .container {
    width: 90%;
    margin: auto;
  }
</style>