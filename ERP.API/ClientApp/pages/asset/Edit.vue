<template>
    <div>
        <h4>Editar Activo</h4>
        <ValidationObserver ref="form">
            <form @submit.prevent="onSubmit">
                <div class="row">
                    <div class="col-3 p-2">
                        <b-button-group class="mt-4 mt-md-0">
                            <b-button variant="secondary" class="btn" size="sm" @click="GoBack">
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
                                <h5 class="text-center">Editar Activo</h5>
                            </div>
                            <div class="card-body justify-content-center container">
                                <div class="row">
                                    <div class="col-md-6">
                                        <b-form-group label="Nombre" label-for="name" :label-cols="4">
                                            <ValidationProvider rules="required" v-slot="{ errors }">
                                                <b-form-input id="name" v-model="asset.name" type="text" size="sm"
                                                    :state="errors.length > 0 ? false : null" />
                                                <span class="text-danger" v-if="errors[0]">Campo requerido</span>
                                            </ValidationProvider>
                                        </b-form-group>
                                    </div>
                                    <div class="col-md-6">
                                        <b-form-group label="Clase de Activo" label-for="assetClassId" :label-cols="4">
                                            <ValidationProvider rules="required" v-slot="{ errors }">
                                                <b-form-select id="assetClassId" v-model="asset.assetClassId"
                                                    :options="assetClasses" size="sm"
                                                    :state="errors.length > 0 ? false : null" />
                                                <span class="text-danger" v-if="errors[0]">Campo requerido</span>
                                            </ValidationProvider>
                                        </b-form-group>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <b-form-group label="Ubicación" label-for="locationId" :label-cols="4">
                                            <ValidationProvider rules="required" v-slot="{ errors }">
                                                <b-form-select id="locationId" v-model="asset.locationId"
                                                    :options="locations" size="sm"
                                                    :state="errors.length > 0 ? false : null" />
                                                <span class="text-danger" v-if="errors[0]">Campo requerido</span>
                                            </ValidationProvider>
                                        </b-form-group>
                                    </div>
                                    <div class="col-md-6">
                                        <b-form-group label="Fecha de Adquisición" label-for="acquisitionDate"
                                            :label-cols="4">
                                            <ValidationProvider rules="required" v-slot="{ errors }">
                                                <b-form-input id="acquisitionDate" v-model="asset.acquisitionDate"
                                                    type="date" size="sm" :state="errors.length > 0 ? false : null" />
                                                <span class="text-danger" v-if="errors[0]">Campo requerido</span>
                                            </ValidationProvider>
                                        </b-form-group>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <b-form-group label="Costo de Adquisición" label-for="acquisitionCost"
                                            :label-cols="4">
                                            <ValidationProvider rules="required|numeric" v-slot="{ errors }">
                                                <b-form-input id="acquisitionCost" v-model="asset.acquisitionCost"
                                                    type="number" size="sm" :state="errors.length > 0 ? false : null" />
                                                <span class="text-danger" v-if="errors[0]">Campo requerido</span>
                                            </ValidationProvider>
                                        </b-form-group>
                                    </div>
                                    <div class="col-md-6">
                                        <b-form-group label="Valor Residual" label-for="residualValue" :label-cols="4">
                                            <b-form-input id="residualValue" v-model="asset.residualValue" type="number"
                                                size="sm" />
                                        </b-form-group>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <b-form-group label="Vida Útil" label-for="usefulLife" :label-cols="4">
                                            <b-form-input id="usefulLife" v-model="asset.usefulLife" type="number"
                                                size="sm" />
                                        </b-form-group>
                                    </div>
                                    <div class="col-md-6">
                                        <b-form-group label="Método de Depreciación" label-for="depreciationMethod"
                                            :label-cols="4">
                                            <b-form-select id="depreciationMethod" v-model="asset.depreciationMethod"
                                                :options="depreciationMethods" size="sm" />
                                        </b-form-group>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <b-form-group label="Número de Serie" label-for="serialNumber" :label-cols="4">
                                            <b-form-input id="serialNumber" v-model="asset.serialNumber" type="text"
                                                size="sm" />
                                        </b-form-group>
                                    </div>
                                    <div class="col-md-6">
                                        <b-form-group label="Marca" label-for="brand" :label-cols="4">
                                            <b-form-input id="brand" v-model="asset.brand" type="text" size="sm" />
                                        </b-form-group>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <b-form-group label="Modelo" label-for="model" :label-cols="4">
                                            <b-form-input id="model" v-model="asset.model" type="text" size="sm" />
                                        </b-form-group>
                                    </div>
                                    <div class="col-md-6">
                      <b-form-group label=" Estado" label-for="depreciationMethod" :label-cols="4">
                        <b-form-select id="depreciationMethod" v-model="asset.Status" :options="statusOptions" size="sm" />
                      </b-form-group>
                    </div>
                                </div>
                                
                                <div class="row">
                                    <div class="col-md-12">
                                        <b-form-group label="Comentario" label-for="commentary" :label-cols="2">
                                            <b-form-textarea id="commentary" v-model="asset.commentary" rows="3"
                                                size="sm" />
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
                            <b-button variant="secondary" class="btn" size="sm" @click="GoBack">
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
export default {
    data() {
        return {
            asset: {
                id: null,
                name: "",
                assetClassId: null,
                locationId: null,
                acquisitionDate: null,
                acquisitionCost: 0,
                residualValue: 0,
                usefulLife: 0,
                depreciationMethod: null,
                status: null,
                serialNumber: "",
                brand: "",
                model: "",
                commentary: "",
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
        };
    },
    async mounted() {
        await this.fetchAssetClasses();
        await this.fetchLocations();
        await this.fetchAsset();
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
                    await this.$axios.put('Asset/Update', this.asset);
                    this.$toast.success("Activo actualizado correctamente.", "Éxito", this.izitoastConfig);
                    this.GoBack();
                }
            } catch (error) {
                this.$toast.error(error.message, "Error al actualizar el activo", this.izitoastConfig);
            }
        },
        async fetchAsset() {
            try {
                const id = this.$route.query.id;
                const response = await this.$axios.get(`Asset/GetById?id=${id}`);
                this.asset = response.data.data; // Asigna directamente la respuesta a la propiedad 'asset'
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