<template>
    <div>
      <input type="file" @change="handleFileUpload" />
      <table v-if="tableData.length">
        <thead>
          <tr>
            <th v-for="(header, index) in tableData[0]" :key="index">{{ header }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(row, rowIndex) in tableData.slice(1)" :key="rowIndex">
            <td v-for="(cell, cellIndex) in row" :key="cellIndex">{{ cell }}</td>
          </tr>
        </tbody>
      </table>
      <button @click="uploadData">Upload Data</button>
    </div>
  </template>
  
  <script>
  import * as XLSX from 'xlsx';
 
  
  export default {
    data() {
      return {
        tableData: []
      };
    },
    methods: {
      handleFileUpload(event) {
        const file = event.target.files[0];
        const reader = new FileReader();
  
        reader.onload = (e) => {
          const data = new Uint8Array(e.target.result);
          const workbook = XLSX.read(data, { type: 'array' });
          const firstSheetName = workbook.SheetNames[0];
          const worksheet = workbook.Sheets[firstSheetName];
          const jsonData = XLSX.utils.sheet_to_json(worksheet, { header: 1 });
          this.tableData = jsonData;
        };
  
        reader.readAsArrayBuffer(file);
      },
      async uploadData() {
        for (let i = 1; i < this.tableData.length; i++) {
          const row = this.tableData[i];
          try {
            await this.$axios.post('https://your-api-endpoint.com/upload', row);
          } catch (error) {
            console.error('Error uploading row:', row, error);
          }
        }
      }
    }
  };
  </script>
  
  <style scoped>
  table {
    width: 100%;
    border-collapse: collapse;
  }
  th, td {
    border: 1px solid #ddd;
    padding: 8px;
  }
  th {
    background-color: #f2f2f2;
  }
  </style>