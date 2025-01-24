<template>
    <div>
        <div class="card-body">
      <div class="d-print-none mt-4 text-center">
        <button @click="exportToCSV" class="btn btn-sm btn-primary">Exportar a CSV</button>
      </div>

      <div
        class="ticket"
        style="font: 14px Lucida Console; background-color: white; color: black"
      >
      
      <!-- Botón para exportar a CSV -->
     
  
      <!-- Primer Componente TablaCuentas -->
      <TablaCuentas
        :accounts="accounts"
        :cash="cash"
        :titleIsActive="titleIsActive"
        @update-cash-difference="handleCashDifference"
        :SubTitle="'INGRESOS'"
      />
  
      <!-- Segundo Componente TablaCuentas -->
      <TablaCuentas
        :accounts="otherAccounts"
        :cash="receivedCashDifference"
        :titleIsActive="titleIsActive"
         :SubTitle="'EGRESOS'"
         :Head="false"
         :Model="1"
        
      />
    </div>  </div>
</div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        accounts: [
          { code: "001", account: "Cuenta A", value: 100 },
          { code: "002", account: "Cuenta B", value: 200 },
          { code: "003", account: "Cuenta C", value: 300 },
        ],
        otherAccounts: [
          { code: "004", account: "Cuenta D", value: 150 },
          { code: "005", account: "Cuenta E", value: 250 },
        ],
        cash: 500,
        titleIsActive: true,
        receivedCashDifference: 0, // Almacena el valor calculado del primer componente
      };
    },
    methods: {
      handleCashDifference(value) {
        // Recibe el valor emitido por el primer componente y lo almacena
        this.receivedCashDifference = value;
      },
      exportToCSV() {
        // Encabezados
        const headers = ["Código", "Cuenta", "Valor"];
        
        // Datos de la primera tabla
        const firstTable = this.accounts.map((item) => [
          item.code,
          item.account,
          item.value,
        ]);
        firstTable.push(["", "TOTAL DE INGRESOS", this.totalIncome(this.accounts)]);
        firstTable.push([
          "",
          "TOTAL INGRESOS + CAJA",
          this.totalIncome(this.accounts) + this.cash,
        ]);
  
        // Datos de la segunda tabla
        const secondTable = this.otherAccounts.map((item) => [
          item.code,
          item.account,
          item.value,
        ]);
        secondTable.push([
          "",
          "TOTAL DE INGRESOS",
          this.totalIncome(this.otherAccounts),
        ]);
        secondTable.push([
          "",
          "TOTAL INGRESOS + CAJA",
          this.totalIncome(this.otherAccounts) + this.receivedCashDifference,
        ]);
  
        // Crear el contenido del CSV
        const csvContent = [
          ["Tabla 1"],
          headers,
          ...firstTable,
          [],
          ["Tabla 2"],
          headers,
          ...secondTable,
        ]
          .map((row) => row.join(",")) // Unir cada fila con comas
          .join("\n"); // Separar filas por saltos de línea
  
        // Crear archivo y descargar
        const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
        const link = document.createElement("a");
        const url = URL.createObjectURL(blob);
        link.setAttribute("href", url);
        link.setAttribute("download", "tablas_cuentas.csv");
        link.style.visibility = "hidden";
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      },
      totalIncome(accounts) {
        return accounts.reduce((sum, item) => sum + item.value, 0);
      },
    },
  };
  </script>
  
  <style>
  button {
    margin-bottom: 20px;
    padding: 10px 20px;
    font-size: 16px;
    cursor: pointer;
  }
  </style>
  