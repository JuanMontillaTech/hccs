<template>
  <div>



  
    <div class="card">

      <div class="card-body">
        <h5 class="card-title">Importar Semestres</h5>
        <p class="card-text">Herramienta para importar los semestres</p>
        <div class="card-body">
          <div class="col-md-6">
            <label for="InstitutionName" class="form-label">Año del semestres:</label>
            <input type="text" class="form-control" id="Year" v-model="formulario.Year">
          </div>

          <div class="row mb-3">
            <div class="col-md-6">
              <label for="InstitutionName" class="form-label">Nombre de la Institución:</label>
              <input type="text" class="form-control" id="InstitutionName" v-model="formulario.InstitutionName">
            </div>
            <div class="col-md-3">
              <label for="Code" class="form-label">Código:</label>
              <input type="text" class="form-control" id="Code" v-model="formulario.Code">
            </div>
            <div class="col-md-3">
              <label for="NumberOfSisters" class="form-label">No. Hermanas:</label>
              <input type="text" class="form-control" id="NumberOfSisters" v-model="formulario.NumberOfSisters">
            </div>
          </div>

          <div class="row mb-3">
            <div class="col-md-4">
              <label for="Country" class="form-label">País:</label>
              <input type="text" class="form-control" id="Country" v-model="formulario.Country">
            </div>
            <div class="col-md-5">
              <label for="City" class="form-label">Ciudad:</label>
              <input type="text" class="form-control" id="City" v-model="formulario.City">
            </div>
            <div class="col-md-3">
              <label for="NumberOfEmployees" class="form-label">No. Empleados:</label>
              <input type="text" class="form-control" id="NumberOfEmployees" v-model="formulario.NumberOfEmployees">
            </div>
          </div>

          <div class="row mb-3">
            <div class="col-md-12">
              <label for="Address" class="form-label">Dirección:</label>
              <input type="text" class="form-control" id="Address" v-model="formulario.Address">
            </div>
          </div>

          <div class="row mb-3">
            <div class="col-md-6">
              <label for="Phone" class="form-label">Teléfono:</label>
              <input type="text" class="form-control" id="Phone" v-model="formulario.Phone">
            </div>
            <div class="col-md-6">
              <label for="Fax" class="form-label">Fax:</label>
              <input type="text" class="form-control" id="Fax" v-model="formulario.Fax">
            </div>
          </div>
          <div class="col-md-6" v-if="btnSend" >
            <label for="InstitutionName" class="form-label">Archivo:</label>
            <input type="file" @change="handleFileUpload" accept=".csv">
          </div>

          

          <div class="w-100 d-flex justify-content-center align-items-center snipper-h"
               v-if="spinner"  >
            <b-spinner style="width: 3rem; height: 3rem"
                       label="Large Spinner"></b-spinner> Registrando
          </div>
          <div v-else>
            <button @click="sendData" class="btn btn-primary" v-if="dataForLoad.length  && btnSend">Enviar</button>
            <table v-if="dataForLoad.length" class="table table-striped">
              <thead>
                <tr>
                  <th v-for="(header, index) in headers" :key="index">{{ header }}</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row, index) in dataForLoad" :key="index">
                  <td v-for="(value, index) in row" :key="index">{{ value }}</td>
                </tr>
              </tbody>
            </table>
         
            <button @click="sendData" class="btn btn-primary" v-if="dataForLoad.length && btnSend" >Enviar</button>
          </div>
        </div>

      </div>
    </div>

  </div>
</template>

<script>
import Papa from 'papaparse';

export default {
  data() {
    return {
      dataForLoad: [],
      headers: [],
      spinner: false,
      btnSend: true,
      formulario: {
        InstitutionName: '',
        Code: '',
        NumberOfSisters: '',
        Country: '',
        City: '',
        NumberOfEmployees: '',
        Address: '',
        Phone: '',
        Fax: '',
        Year: 0,
      }
    };
  },
  methods: {

    handleFileUpload(event) {
      const file = event.target.files[0];
      Papa.parse(file, {
        header: true,
        dynamicTyping: true,
        complete: (results) => {
          this.headers = results.meta.fields;
          this.dataForLoad = results.data.map(row => ({
            tipo: parseInt(row.TIPO === null || row.TIPO === undefined || row.TIPO === '' ? 0 : row.TIPO),           
            cod: row.COD === null || row.COD === undefined || row.COD === '' ? '' : '' + row.COD,
            cuenta: row.CUENTA === null || row.CUENTA === undefined || row.CUENTA === '' ? '' : row.CUENTA,
            enero: parseFloat(row.ENERO === null || row.ENERO === undefined || row.ENERO === '' ? 0 : row.ENERO),
            febrero: parseFloat(row.FEBRERO === null || row.FEBRERO === undefined || row.FEBRERO === '' ? 0 : row.FEBRERO),
            marzo: parseFloat(row.MARZO === null || row.MARZO === undefined || row.MARZO === '' ? 0 : row.MARZO),
            abril: parseFloat(row.ABRIL === null || row.ABRIL === undefined || row.ABRIL === '' ? 0 : row.ABRIL),
            mayo: parseFloat(row.MAYO === null || row.MAYO === undefined || row.MAYO === '' ? 0 : row.MAYO),
            junio: parseFloat(row.JUNIO === null || row.JUNIO === undefined || row.JUNIO === '' ? 0 : row.JUNIO),
            julio: parseFloat(row.JULIO === null || row.JULIO === undefined || row.JULIO === '' ? 0 : row.JULIO),
            agosto: parseFloat(row.AGOSTO === null || row.AGOSTO === undefined || row.AGOSTO === '' ? 0 : row.AGOSTO),
            septiembre: parseFloat(row.SEPTIEMBRE === null || row.SEPTIEMBRE === undefined || row.SEPTIEMBRE === '' ? 0 : row.SEPTIEMBRE),
            octubre: parseFloat(row.OCTUBRE === null || row.OCTUBRE === undefined || row.OCTUBRE === '' ? 0 : row.OCTUBRE),
            noviembre: parseFloat(row.NOVIEMBRE === null || row.NOVIEMBRE === undefined || row.NOVIEMBRE === '' ? 0 : row.NOVIEMBRE),
            diciembre: parseFloat(row.DICIEMBRE === null || row.DICIEMBRE === undefined || row.DICIEMBRE === '' ? 0 : row.DICIEMBRE),
          }));
        },
      });
    },

    sendData() {
      this.spinner = true;
      this.btnSend = false;
      let url = "Import/LoadCSV"

      let data = {
        headSemesters: this.formulario,
        data: this.dataForLoad
    
      };
      this.$axios
        .post(url, data)
        .then((response) => {
          this.$toast.success(
            "Registro guardado.",
            "Notificación"
          );
         
          this.spinner = false;
          this.btnSend = false;

        })
        .catch((error) => {
          this.spinner = false;
          this.btnSend = true;
          this.$toast.error(`${error}`, "ERROR");
        });
    },
  },
};
</script>
