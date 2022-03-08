<template>
    <div class="container" style="height: 820px;">
        <!-- Modal for create a contact -->
        <b-modal size="lg" title="Formulario de Contacto" header-bg-variant="#000" v-model="ShowModalCreate" hide-footer>
            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-6">
                        <b-form-group
                            label="Tipo de identificación"
                            >
                            <b-form-select v-model="contact.IdentificationType" :options="options" size="sm" ></b-form-select>
                        </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                :label="contact.IdentificationType == null ? 'RNC o Cédula' :`${contact.IdentificationType} número`"
                                >
                                <b-form-input v-model="contact.DocumentNumber" size="sm"  trim></b-form-input>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Nombre/Razón social"
                                >
                                <b-form-input v-model="contact.Name" size="sm" :state="$v.contact.Name.$error ? false : null" trim></b-form-input>
                                <p class="text-danger text-size-required m-0" v-if="$v.contact.Name.$error">Nombre/Razón social requerido.</p>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                        <b-form-group
                            label="Municipio / Provincia"
                            >
                            <b-form-select v-model="contact.Province" :options="provinces" size="sm" ></b-form-select>
                        </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-12">
                            <b-form-group
                                label="Dirección"
                                >
                                <b-form-input v-model="contact.Address" size="sm"  trim></b-form-input>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Correo electrónico"
                                >
                                <b-form-input v-model="contact.Email" size="sm" :state="$v.contact.Email.$error ? false : null" trim></b-form-input>
                                <p class="text-danger text-size-required m-0" v-if="$v.contact.Email.$error">Formato de email incorrecto.</p>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Celular"
                                >
                                <b-form-input v-model="contact.CellPhone" size="sm" trim></b-form-input>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Teléfono 1"
                                >
                                <b-form-input v-model="contact.Phone1" size="sm" trim></b-form-input>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Teléfono 2"
                                >
                                <b-form-input v-model="contact.Phone2" size="sm" trim></b-form-input>
                            </b-form-group>
                    </div>

                    <div class="col-sm-12 col-md-6">
                        <b-form-group label="Tipo de contacto">
                            <b-form-checkbox
                                v-model="contact.IsClient"
                                :value="true"
                                :unchecked-value="false"
                                >
                                Cliente
                            </b-form-checkbox>

                            <b-form-checkbox
                                v-model="contact.IsSupplier"
                                name="checkbox-1"
                                :value="true"
                                :unchecked-value="false"
                                >
                                Proveedor
                            </b-form-checkbox>
                        </b-form-group>
                    </div>

                    <div class="row justify-content-end w-100 gx-2">
                        <div class="col-2 p-2">
                            <b-button variant="danger" class="w-100" @click="ShowModalCreate = !ShowModalCreate">Cancelar</b-button>
                        </div>
                        <div class="col-3 p-2">
                            <b-button class="w-100" style="background-color: #457b9d;" @click="saveContact()">
                                <span>Crear contacto</span> 
                            </b-button>
                        </div>
                    </div>
                </div>
            </div>
        </b-modal>

        <!-- Modal for show contact details -->
        <b-modal size="lg" title="Formulario de Contacto" v-model="ShowModalDetails" hide-footer>
            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-6">
                        <b-form-group
                            label="Tipo de identificación"
                            >
                            <b-form-select v-model="contact.IdentificationType" :options="options" size="sm" disabled></b-form-select>
                        </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                :label="contact.IdentificationType == null ? 'RNC o Cédula' :`${contact.IdentificationType} número`"
                                >
                                <b-form-input v-model="contact.DocumentNumber" size="sm"  trim disabled></b-form-input>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Nombre/Razón social"
                                >
                                <b-form-input v-model="contact.Name" size="sm" :state="$v.contact.Name.$error ? false : null" trim disabled></b-form-input>
                                <p class="text-danger text-size-required m-0" v-if="$v.contact.Name.$error">Nombre/Razón social requerido.</p>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                        <b-form-group
                            label="Municipio / Provincia"
                            >
                            <b-form-select v-model="contact.Province" :options="provinces" size="sm" disabled></b-form-select>
                        </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-12">
                            <b-form-group
                                label="Dirección"
                                >
                                <b-form-input v-model="contact.Address" size="sm"  trim disabled></b-form-input>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Correo electrónico"
                                >
                                <b-form-input v-model="contact.Email" size="sm" :state="$v.contact.Email.$error ? false : null" trim disabled></b-form-input>
                                <p class="text-danger text-size-required m-0" v-if="$v.contact.Email.$error">Formato de email incorrecto.</p>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Celular"
                                >
                                <b-form-input v-model="contact.CellPhone" size="sm" trim disabled></b-form-input>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Teléfono 1"
                                >
                                <b-form-input v-model="contact.Phone1" size="sm" trim disabled></b-form-input>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Teléfono 2"
                                >
                                <b-form-input v-model="contact.Phone2" size="sm" trim disabled></b-form-input>
                            </b-form-group>
                    </div>
                    
                    <div class="col-sm-12 col-md-6">
                        <b-form-group label="Tipo de contacto">
                            <b-form-checkbox
                                v-model="contact.IsClient"
                                :value="true"
                                :unchecked-value="false"
                                disabled
                                >
                                Cliente
                            </b-form-checkbox>

                            <b-form-checkbox
                                v-model="contact.IsSupplier"
                                name="checkbox-1"
                                :value="true"
                                :unchecked-value="false"
                                disabled
                                >
                                Proveedor
                            </b-form-checkbox>
                        </b-form-group>
                    </div>

                    <div class="row justify-content-end w-100 gx-2">
                        <div class="col-2 p-2">
                            <b-button variant="danger" class="w-100" @click="ShowModalDetails = !ShowModalDetails">Cancelar</b-button>
                        </div>
                    </div>
                </div>
            </div>
        </b-modal>

        <!-- Modal for update contact -->
                <b-modal size="lg" title="Formulario de Contacto" v-model="ShowModalEdit" hide-footer>
            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-6">
                        <b-form-group
                            label="Tipo de identificación"
                            >
                            <b-form-select v-model="contact.IdentificationType" :options="options" size="sm" ></b-form-select>
                        </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                :label="contact.IdentificationType == null ? 'RNC o Cédula' :`${contact.IdentificationType} número`"
                                >
                                <b-form-input v-model="contact.DocumentNumber" size="sm"  trim></b-form-input>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Nombre/Razón social"
                                >
                                <b-form-input v-model="contact.Name" size="sm" :state="$v.contact.Name.$error ? false : null" trim></b-form-input>
                                <p class="text-danger text-size-required m-0" v-if="$v.contact.Name.$error">Nombre/Razón social requerido.</p>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                        <b-form-group
                            label="Municipio / Provincia"
                            >
                            <b-form-select v-model="contact.Province" :options="provinces" size="sm" ></b-form-select>
                        </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-12">
                            <b-form-group
                                label="Dirección"
                                >
                                <b-form-input v-model="contact.Address" size="sm"  trim></b-form-input>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Correo electrónico"
                                >
                                <b-form-input v-model="contact.Email" size="sm" :state="$v.contact.Email.$error ? false : null" trim></b-form-input>
                                <p class="text-danger text-size-required m-0" v-if="$v.contact.Email.$error">Formato de email incorrecto.</p>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Celular"
                                >
                                <b-form-input v-model="contact.CellPhone" size="sm" trim></b-form-input>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Teléfono 1"
                                >
                                <b-form-input v-model="contact.Phone1" size="sm" trim></b-form-input>
                            </b-form-group>
                    </div>
                    <div class="col-sm-12 col-md-6">
                            <b-form-group
                                label="Teléfono 2"
                                >
                                <b-form-input v-model="contact.Phone2" size="sm" trim></b-form-input>
                            </b-form-group>
                    </div>

                    <div class="col-sm-12 col-md-6">
                        <b-form-group label="Tipo de contacto">
                            <b-form-checkbox
                                v-model="contact.IsClient"
                                :value="true"
                                :unchecked-value="false"
                                >
                                Cliente
                            </b-form-checkbox>

                            <b-form-checkbox
                                v-model="contact.IsSupplier"
                                :value="true"
                                :unchecked-value="false"
                                >
                                Proveedor
                            </b-form-checkbox>
                        </b-form-group>
                    </div>

                    <div class="row justify-content-end w-100 gx-2">
                        <div class="col-2 p-2">
                            <b-button variant="danger" class="w-100" @click="ShowModalEdit = !ShowModalEdit">Cancelar</b-button>
                        </div>
                        <div class="col-3 p-2">
                            <b-button class="w-100" style="background-color: #457b9d;" @click="editContact()">
                                <span>Guardar cambios</span> 
                            </b-button>
                        </div>
                    </div>
                </div>
            </div>
        </b-modal>

        <div class="card">
            <div class="card-header bg-Cprimary">
                Listado de {{ $options.name }}
            
            </div>
            <div class="card-body">
                <div class="btn-group" role="group" aria-label="Basic example">
                <a
                    title="Nuevo Registro"
                    @click="showModal()"
                    class="btn btn-primary btn-sm text-white"
                    >
                    <fa icon="file" class="ml-1"></fa>
                    Nuevo</a
                >

                <a
                    id="_btnRefresh"
                    @click="GetAllRows()"
                    class="btn btn-light btn-sm text-black-50 btnRefresh"
                    name="_btnRefresh"
                    ><i class="fas fa-sync-alt"></i> Actualizar Datos</a
                >
                </div>
                <vue-good-table
                :columns="columns"
                :rows="rows"
                :search-options="{
                    enabled: true,
                }"
                :pagination-options="{
                    enabled: true,
                    mode: 'records',
                }"
                >
                <template slot="table-row" slot-scope="props">
                    <span v-if="props.column.field == 'action'">
                    <b-button
                        class="btn btn-light btn-sm"
                        @click="showContact(props.row)"
                    >
                        <i class="fa fa-eye"></i>
                    </b-button>
                    <b-button
                        class="btn btn-light btn-sm"
                        @click="removeContact(props.row)"
                    >
                        <i class="fa fa-trash"></i>
                    </b-button>
                    <b-button
                        class="btn btn-light btn-sm"
                        @click="editContactModal(props.row)"
                    >
                        <i class="fa fa-edit"></i
                    ></b-button>
                    </span>
                    <span v-else>
                    {{ props.formattedRow[props.column.field] }}
                    </span>
                </template>
                </vue-good-table>
            </div>
        </div>
    </div>
</template>

<script>
import { required, email } from "vuelidate/lib/validators";
export default {
        name: "Clientes",
        layout: 'TheSlidebar',
        data() {
            return {
                ShowModalCreate: false,
                ShowModalEdit: false,
                ShowModalDelete: false,
                ShowModalDetails: false,
                contact: {
                    IdentificationType: '',
                    DocumentNumber: '',
                    Name: '',
                    Address: '',
                    Province: '',
                    Email: '',
                    CellPhone: '',
                    Phone1: '',
                    Phone2: '',
                    IsClient: true,
                    IsSupplier: false

                },
                izitoastConfig: {
                    position: "topRight",
                },
                provinces: [
                    {
                        value : 1,
                        text : 'Distrito Nacional'
                    },
                    {
                        value : 21,
                        text : 'San Pedro de Macorís'
                    },
                    {
                        value : 22,
                        text : 'La Romana'
                    },
                    {
                        value : 23,
                        text : 'La Altagracia'
                    },
                    {
                        value : 24,
                        text : 'El Seibo'
                    },
                    {
                        value : 25,
                        text : 'Hato Mayor'
                    },
                    {
                        value : 31,
                        text : 'Azua'
                    },
                    {
                        value : 32,
                        text : 'Samaná'
                    },
                    {
                        value : 33,
                        text : 'Maria Trinidad Sánchez'
                    },
                    {
                        value : 34,
                        text : 'Salcedo'
                    },
                    {
                        value : 41,
                        text : 'La Vega'
                    },
                    {
                        value : 42,
                        text : 'Monseñor Nouel'
                    },
                    {
                        value : 43,
                        text : 'Sánchez Ramirez'
                    },
                    {
                        value : 51,
                        text : 'Santiago'
                    },
                    {
                        value : 56,
                        text : 'Espaillat'
                    },
                    {
                        value : 57,
                        text : 'Puerto Plata'
                    },
                    {
                        value : 61,
                        text : 'Valverde'
                    },
                    {
                        value : 62,
                        text : 'Monte Cristi'
                    },
                    {
                        value : 63,
                        text : 'Dajabónn'
                    },
                    {
                        value : 64,
                        text : 'Santiago Rodríguez'
                    },
                    {
                        value : 71,
                        text : 'Azua'
                    },
                    {
                        value : 72,
                        text : 'San Juan de la Maguana'
                    },
                    {
                        value : 73,
                        text : 'Elías Piña'
                    },
                    {
                        value : 81,
                        text : 'Barahona'
                    },
                    {
                        value : 82,
                        text : 'Bahoruco'
                    },
                    {
                        value : 83,
                        text : 'Independencia'
                    },
                    {
                        value : 84,
                        text : 'Perdenales'
                    },
                    {
                        value : 91,
                        text : 'San Cristóbal'
                    },
                    {
                        value : 92,
                        text : 'Monte Plata'
                    },
                    {
                        value : 93,
                        text : 'San José de Ocoa'
                    },
                    {
                        value : 94,
                        text : 'Peravia'
                    }
                ],
                selected: null,
                options: [
                    { value: 'RNC', text: 'RNC' },
                    { value: 'Cédula', text: 'Cédula' },
                    { value: 'Pasaporte', text: 'Pasaporte (Identificador extranjero)' }
                ],
                columns: [
                    {
                        label: 'Nombre/Razón social',
                        field: 'Name',
                    },
                    {
                        label: 'Identificación',
                        field: 'IdentificationType',
                        type: 'number',
                    },
                    {
                        label: 'Télefono ',
                        field: 'CellPhone',
                    },
                    {
                        label: 'Acciones',
                        field: 'action',
                    },
                ],
                rows: [
                    { id: 1, IdentificationType: 'RNC', DocumentNumber: '0001', Name: 'EXAMPLE 1', Address: 'ADDRESS 1', Province: 'Azua', Email: 'EMAIL1@GMAIL.COM', CellPhone: '982384', Phone1: '092343', Phone2: '102302', IsClient: true, IsSupplier: true},
                    { id: 2, IdentificationType: 'RNC', DocumentNumber: '0002', Name: 'EXAMPLE 2', Address: 'ADDRESS 1', Province: 'Azua', Email: 'EMAIL2@GMAIL.COM', CellPhone: '982384', Phone1: '092343', Phone2: '102302', IsClient: true, IsSupplier: true},
                    { id: 3, IdentificationType: 'RNC', DocumentNumber: '0003', Name: 'EXAMPLE 3', Address: 'ADDRESS 1', Province: 'Azua', Email: 'EMAIL3@GMAIL.COM', CellPhone: '982384', Phone1: '092343', Phone2: '102302', IsClient: true, IsSupplier: true},
                    { id: 4, IdentificationType: 'Cédula', DocumentNumber: '40200694152', Name: 'EXAMPLE 4', Address: 'ADDRESS 1', Province: 'Azua', Email: 'EMAIL4@GMAIL.COM', CellPhone: '092343', Phone1: '934843', Phone2: '102302', IsClient: true, IsSupplier: true},
                    { id: 5, IdentificationType: 'Cédula', DocumentNumber: '40200694152', Name: 'EXAMPLE 5', Address: 'ADDRESS 1', Province: 'Azua', Email: 'EMAIL5@GMAIL.COM', CellPhone: '092343', Phone1: '934843', Phone2: '102302', IsClient: true, IsSupplier: true},
                    { id: 6, IdentificationType: 'Cédula', DocumentNumber: '40200694152', Name: 'EXAMPLE 6', Address: 'ADDRESS 1', Province: 'Azua', Email: 'EMAIL6@GMAIL.COM', CellPhone: '092343', Phone1: '934843', Phone2: '102302', IsClient: true, IsSupplier: true},
                    { id: 7, IdentificationType: 'Pasaporte', DocumentNumber: 'AA0323', Name: 'EXAMPLE 7', Address: 'ADDRESS 1', Province: 'Azua', Email: 'EMAIL7@GMAIL.COM', CellPhone: '092343', Phone1: '934843', Phone2: '102302', IsClient: true, IsSupplier: true},
                    { id: 8, IdentificationType: 'Pasaporte', DocumentNumber: 'AA0323', Name: 'EXAMPLE 8', Address: 'ADDRESS 1', Province: 'Azua', Email: 'EMAIL8@GMAIL.COM', CellPhone: '092343', Phone1: '934843', Phone2: '102302', IsClient: true, IsSupplier: true},
                    { id: 9, IdentificationType: 'Pasaporte', DocumentNumber: 'AA0323', Name: 'EXAMPLE 9', Address: 'ADDRESS 1', Province: 'Azua', Email: 'EMAIL9@GMAIL.COM', CellPhone: '092343', Phone1: '934843', Phone2: '102302', IsClient: true, IsSupplier: true},
                    { id: 10, IdentificationType: 'Pasaporte', DocumentNumber: 'AA0323', Name: 'EXAMPLE 10', Address: 'ADDRESS 1', Province: 'Azua', Email: 'EMAIL10@GMAIL.COM', CellPhone: '092343', Phone1: '934843', Phone2: '102302', IsClient: true, IsSupplier: true},
                ],
            };
        },
        validations: {
            contact: {
                Name: {
                    required,
                },
                Email: {
                    email
                }
            }
        },
        created() {
            this.GetAllRows();
        },
        methods: {
            GetAllRows(){
                console.log('getall');
            },
            showModal(){
                this.ShowModalCreate = true;
            },
            saveContact(){
                this.$v.$touch();
                if (this.$v.$invalid) {
                    this.$toast.error(
                    "Por favor complete el formulario correctamente.",
                    "ERROR",
                    this.izitoastConfig
                    );
                } else {
                    this.$toast.success(
                        "El Contacto ha sido creado correctamente.",
                        "EXITO",
                        this.izitoastConfig
                    );
                    this.ShowModalCreate = false;
                    this.post(this.contact)
                    console.log(this.contact);
                }
            },
            showContact(contact){
                this.contact = contact;
                this.ShowModalDetails = true;
                console.log(contact);
            },
            removeContact(contact){
                this.$toast.success(
                    "El Contacto ha sido eliminado correctamente.",
                    "EXITO",
                    this.izitoastConfig
                );
                this.delete(contact.id);
            },
            editContactModal(contact){
                this.ShowModalEdit = true;
                this.contact = contact;
                console.log(contact);
            },
            editContact(){
                this.put(this.contact);
            },
            async post(data){
                let url = '';
                    return new Promise((resolve, reject) => {
                        axios
                            .post(url, data, {
                                headers: {
                                    "Content-Type": "application/json",
                                },
                            })
                            .then((response) => {
                                resolve(response);
                                this.$toast.success(
                                "El Contacto ha sido creado correctamente.",
                                "EXITO",
                                this.izitoastConfig
                            );
                            })
                            .catch((error) => {
                                reject(error);
                                this.$toast.error(
                                `${error}`,
                                "ERROR",
                                this.izitoastConfig
                                );
                            });
                    });
            },
            async put(data){
                let url = '';
                return new Promise((resolve, reject) => {
                    axios
                        .put(url, data, {
                            headers: {
                                "Content-Type": "application/json",
                            },
                        })
                        .then((response) => {
                            resolve(response);
                            this.$toast.success(
                                "El Contacto ha sido actualizado correctamente.",
                                "EXITO",
                                this.izitoastConfig
                            );
                        })
                        .catch((error) => {
                            reject(error);
                            this.$toast.error(
                                `${error}`,
                                "ERROR",
                                this.izitoastConfig
                            );
                        });
                });
            },
            async delete(id){
                let url = `/${id}`;
                return new Promise((resolve, reject) => {
                    axios
                        .delete(url, {
                            headers: {
                                "Content-Type": "application/json",
                            },
                        })
                        .then((response) => {
                            resolve(response);
                            this.$toast.success(
                                "El Contacto ha sido eliminado correctamente.",
                                "EXITO",
                                this.izitoastConfig
                            );
                        })
                        .catch((error) => {
                            reject(error);
                            this.$toast.error(
                                `${error}`,
                                "ERROR",
                                this.izitoastConfig
                            );
                        });
                });
            },
            clearForm(){
                for (const key in this.contact) {
                    this.contact[key] = '';
                }
                this.ShowModalCreate = false;
                console.log(this.contact);
            }
        },
    };
</script>

<style>
    .modal-header {
        background-color: #457b9d !important;
        color: #fff;
    }
    .text-size-required {
        font-size: 12px;
    }
</style>
