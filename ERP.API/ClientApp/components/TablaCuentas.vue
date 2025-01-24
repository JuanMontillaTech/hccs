<template>
    <div>
      <table border="1" style="width: 300px;" class="table table-bordered">
        <!-- Encabezado -->
        <thead v-if="Head">
          <tr>
            <th>Nombre de Cuenta</th>
            <th>Enero</th>
          </tr>
        </thead>
  
        <tbody>
          <!-- Caja disponible -->
          <tr>
            <th  v-if="titleIsActive" >Disponible en Caja</th>
            
            <td  colspan="2">{{Money( cash) }}</td>
          </tr>
          <tr>
          <th colspan="2" v-if="SubTitle && titleIsActive === true" >{{ SubTitle }}</th>
        </tr>
  
          <!-- Cuentas, códigos y valores -->
          <tr v-for="(item, index) in accounts" :key="index">
            <th style="width: 25%;">{{ item.code }}</th>
            <th style="width: 50%;">{{ item.account }}</th>
            <td style="width: 25%;">{{ Money( item.value) }}</td>
          </tr>
        </tbody>
  
        <!-- Pie de tabla -->
        <tfoot>
          <tr>
            <th  v-if="titleIsActive" >TOTAL DE {{ SubTitle }}</th>
            <td colspan="2">{{ Money( totalIncome) }}</td>
          </tr>
          <tr v-if="Model == 0">        
            <th  v-if="titleIsActive" >TOTAL {{ SubTitle }}  + CAJA</th>
            <td colspan="2">{{Money(  cashDifference) }}</td>
          </tr>
        </tfoot>
      </table>
    </div>
  </template>
  
  <script>
var numbro = require("numbro");
var moment = require("moment");
export default {
  name: "TablaCuentas",
  props: {
    accounts: {
      type: Array,
      required: true,
      default: () => [] // Estructura: [{ code: "", account: "", value: 0 }]
    },
    cash: {
      type: Number,
      required: false,
      default: 0
    },
    Model: {
      type: Number,
      required: false,
      default: 0
    },
    SubTitle: {
      type: String,
      required: false,
      default: null
    },
    Head: {
        type: Boolean,
      required: true,
      default: true
    },
    titleIsActive: {
      type: Boolean,
      required: true,
      default: true
    }
  },
  computed: {
   
    totalIncome() {
      return this.accounts.reduce((sum, item) => sum + item.value, 0);
    },
    cashDifference() {
        if (this.Model == 0) {
            return this.totalIncome + this.cash;
        }
        else {
            return this.totalIncome;
        }
     
    }
  },
  watch: {
    // Observa cambios en cashDifference y emite el valor al componente padre
    totalIncome: {
      immediate: true, // Emitir al montar el componente
      handler() {
        this.$emit('update-cash-difference', this.cashDifference);
      }
    },
    cash: {
      immediate: true, // Emitir al montar el componente
      handler() {
        this.$emit('update-cash-difference', this.cashDifference);
      }
    }
  },
  methods:{
    Money(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },
  }
};
</script>


  
  <style scoped>
  /* Puedes personalizar el estilo de la tabla aquí */
  table {
    width: 100%;
    border-collapse: collapse; 
    text-align: left;
  }
   td  {
    text-align: right;
  }
  th { 
    text-align: left;
  }
  </style>
  