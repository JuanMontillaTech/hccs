<template>
  <div>
    <input type="file" @change="handleFileUpload2" />
    <div v-if="csvData.length">
      <table>
        <thead>
          <tr>
            <th v-for="header in csvHeaders">{{ header }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="row in csvData">
            <td v-for="cell in row">{{ cell }}</td>
          </tr>
        </tbody>
      </table>
      <button @click="handleUpload" :disabled="!selectedFile || isLoading">
        {{ isLoading ? "Cargando..." : "Cargar CSV" }}
      </button>

      <div v-if="isLoading">
        <progress :value="uploadProgress" max="100"></progress>
        <p>{{ uploadProgress }}%</p>
      </div>
    </div>
  </div>
</template>

<script>
import Papa from "papaparse";

export default {
  data() {
    return {
      csvData: [],
      csvHeaders: [],
      isLoading: false,
      uploadProgress: 0,
      selectedFile: null, // Archivo seleccionado
    };
  },
  methods: {
    handleFileSelect(event) {
      this.selectedFile = event.target.files[0];
    },
    async handleFileUpload2(event) {
      this.isLoading = true; // Iniciar indicador de carga
      this.uploadProgress = 0; // Reiniciar progreso

      const file = event.target.files[0];
      const totalRows = await this.getCSVRowCount(file); // Obtener total de filas
      this.selectedFile = event.target.files[0];

      Papa.parse(file, {
        header: true,
        skipEmptyLines: true,
        complete: (results) => {
          this.csvData = results.data;
          this.csvHeaders = results.meta.fields;
          console.log("All rows processed.");
          this.isLoading = false;
        },
        step: async (row, parser) => {
          try {
            console.log(row.data.Code);
            const accountingImport = {
              code: row.data.Code,
              name: row.data.Name,
              january: parseFloat(row.data.January),
              february: parseFloat(row.data.February),
              march: parseFloat(row.data.March),
              april: parseFloat(row.data.April),
              may: parseFloat(row.data.May),
              june: parseFloat(row.data.June),
              july: parseFloat(row.data.July),
              august: parseFloat(row.data.August),
              september: parseFloat(row.data.September),
              october: parseFloat(row.data.October),
              november: parseFloat(row.data.November),
              december: parseFloat(row.data.December),
            };
            console.log("Data sent successfully:", accountingImport);
            var url = "Accountant/AccountingImport";
            this.$axios.post(url, accountingImport);

            // Actualizar progreso
            this.uploadProgress = Math.round(
              (parser.meta.lines / totalRows) * 100
            );

            console.log("Data sent successfully:", accountingImport);
          } catch (error) {
            console.error(
              "Error sending data:",
              error.response ? error.response.data : error.message
            );
            // Manejar errores (mostrar mensaje al usuario, detener el proceso, etc.)
            parser.abort(); // Detener el parseo en caso de error
          }
        },

        error: (error) => {
          console.error("Error parsing CSV:", error);
          this.isLoading = false; // Detener indicador de carga
          // Mostrar mensaje de error al usuario
        },
      });
    },

    async getCSVRowCount(file) {
      return new Promise((resolve, reject) => {
        const reader = new FileReader();

        reader.onload = (event) => {
          const csvText = event.target.result;
          const rowCount = csvText.split(/\r\n|\n|\r/).length;
          resolve(rowCount);
        };

        reader.onerror = () => {
          reject(new Error("Error al leer el archivo CSV"));
        };

        reader.readAsText(file);
      });
    },

    handleFileUpload(event) {
      const file = event.target.files[0];

      Papa.parse(file, {
        header: true,
        skipEmptyLines: true,
        complete: (results) => {
          this.csvData = results.data;
          this.csvHeaders = results.meta.fields;
        },
        error: (error) => {
          console.error("Error parsing CSV:", error);
        },
      });
    },
  },
};
</script>
