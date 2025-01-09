<template>
  <div class="container-fluid">
    <div class="row">
      <div class="col-md-6">
        <div class="card">
          <div class="card-header">Ticket</div>
          <div class="card-body">
            <div>
              Cliente:
              <vSelectContact :field="clientField" @CustomChange="GetLitValue"></vSelectContact>
              <span v-if="selectedContact">{{ selectedContact.name }}</span>
            </div>
            <div class="table-responsive" style="max-height: 600px; overflow-y: auto;">
              <table class="table table-sm table-striped">
                <thead>
                  <tr>
                    <th style="width: 85%;text-align: left;">Descripción</th>
                    <th style="width: 5%; text-align: right;">Cantidad</th>
                    <th style="width: 5%; text-align: right;">Precio</th>
                    <th style="width: 5%; text-align: right;">Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(ticketItem, index) in ticket" :key="index">
                    <td style="width: 85%;text-align: left;">{{ ticketItem.description }}</td>
                    <td style="width: 5%; text-align: right;">
                      <input type="number" class="form-control form-control-sm" v-model.number="ticketItem.quantity"
                        @input="updateTotal" min="1">
                    </td>
                    <td style="width: 5%; text-align: right;">$ {{ ticketItem.price }}</td>
                    <td style="width: 5%; text-align: right;">
                      <button @click="removeItem(index)" class="btn btn-sm btn-danger">-</button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
            <div class="mt-2">
              <label for="discount">Descuento:</label>
              <input type="number" id="discount" class="form-control form-control-sm" v-model.number="discount"
                @input="updateTotal" min="0">
            </div>
            <p class="mt-2">
              <b>Total: ${{ totalWithDiscount }}</b>
            </p>
           
            <button @click="showModal = true" class="btn btn-success mt-2">Pagar</button>
          </div>
        </div>
      </div>
      <div class="col-md-6">
        <div class="card">
          <div class="card-header">Artículos</div>
          <div class="card-body">
            <div class="mb-2">
              <input type="text" v-model="searchQuery" placeholder="Buscar artículo..."
                class="form-control form-control-sm">
            </div>
            <div class="table-responsive" style="max-height: 600px; overflow-y: auto;">
              <b-table striped hover :items="filteredArticles" :per-page="perPage" :current-page="currentPage"
                :fields="fields" responsive="sm">
                <template #cell(priceSale)="data">
                  <button @click="addItemToTicket(data.item)" class="btn btn-lg btn-primary w-100">
                    <div class="alert alert-success" role="alert">
                      {{ data.item.description }}
                    </div>
                    <div class="alert alert-info" role="alert">
                      ${{ data.value }}
                    </div>
                  </button>
                </template>
              </b-table>
            </div>
            <b-pagination v-model="currentPage" :total-rows="totalRows" :per-page="perPage"></b-pagination>
          </div>
        </div>
      </div>
    </div>

    <b-modal v-model="showModal" title="Pagos" hide-footer size="xl">
      <div class="row">
        <div class="col-md-6">

          <table class="table striped table-border">
            <thead class="bg-Cprimary">
              <tr>
                <th style="width: 20%">Denominación</th>

                <th style="width: 20%">Cantidad</th>
                <th style="width: 20%">Total</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(denomination, index) in form.denominations" v-bind:key="index">
                <td>{{ denomination.name }}</td>

                <td>
                  <input type="number" v-model="denomination.quantity" @input="calculateTotal">
                </td>
                <td>${{ denomination.total }}</td>

              </tr>
            </tbody>

          </table>
        </div>

        <div class="col-md-6">
          <div class="card">
            <div class="card-header">Métodos de Pago</div>
            <div class="card-body">
              <div class="list-group">
                <button type="button" class="list-group-item list-group-item-action" v-for="method in paymentMethods"
                  :key="method.name" @click="addDenomination(totalWithDiscount)">
                  {{ method.name }}
                </button>
              </div>
            </div>
          </div>
          <div class="card">
            <div class="card-header">RESUMEN DE PAGO</div>
            <div class="card-body">
              <p>Total a pagar: ${{ totalWithDiscount }}</p>
              <p>Total recibido: ${{ totalToSubtract }}</p>
              <p v-if="totalToSubtract > totalWithDiscount">
              <div class="alert alert-warning" role="alert">
                Devuelta:${{ (totalWithDiscount - totalToSubtract) }}</div>

              </p>
              <p v-if="totalWithDiscount < totalToSubtract">
              <div class="alert alert-warning" role="alert">
                Pendiente a pagar:${{ (totalWithDiscount - totalToSubtract) }}
              </div>

              </p>
              <button @click="printTicket" class="btn btn-primary mt-2">Imprimir Ticket</button>
            </div>
          </div>
        </div>
      </div>


    </b-modal>
  </div>
</template>

<script>
export default {
  data() {
    return {
      articles: [],
      ticket: [],
      searchQuery: '',
      clientField: {
        field: "selectedContact",
        sourceApi: "Contact/GetFilter",
        sourceLabel: "name",
        formSoportId: "1534f256-6c42-41ea-860b-ae8f69206865"
      },
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      selectedContact: null,
      discount: 0,
      currentPage: 1,
      fields: [
        { key: 'priceSale', label: '' },
        { key: 'Acciones', label: '' }
      ],
      showModal: false,
      form: {
        denominations: [
          { id: null, name: '2000', value: 2000, quantity: 0, total: 0 },
          { id: null, name: '1000', value: 1000, quantity: 0, total: 0 },
          { id: null, name: '500', value: 500, quantity: 0, total: 0 },
          { id: null, name: '100', value: 100, quantity: 0, total: 0 },
          { id: null, name: '50', value: 50, quantity: 0, total: 0 },
          { id: null, name: '25', value: 25, quantity: 0, total: 0 },
          { id: null, name: '10', value: 10, quantity: 0, total: 0 },
          { id: null, name: '5', value: 5, quantity: 0, total: 0 },
          { id: null, name: '1', value: 1, quantity: 0, total: 0 },
        ],
      },

      paymentMethods: [
        { name: 'Cash' },
        { name: 'Credit' },
        { name: 'Debit' },
        { name: 'Gift Card' }
      ],
      totalToSubtract: 0,
      amountReceived: 0
    };
  },
  computed: {
    filteredArticles() {
      let filtered = this.articles.filter(item => {
        return item.description.toLowerCase().includes(this.searchQuery.toLowerCase())
      });

      if (!isNaN(parseFloat(this.searchQuery)) && isFinite(this.searchQuery)) {
        const price = parseFloat(this.searchQuery);
        filtered = filtered.filter(item => item.priceSale === price);
      }

      return filtered
    },
    total() {
      return this.ticket.reduce((sum, item) => sum + item.price * item.quantity, 0);
    },
    totalWithDiscount() {
      const total = this.total;
      const discountAmount = (this.discount / 100) * total;
      return (total - discountAmount).toFixed(2);
    },
    totalRows() {
      return this.filteredArticles.length;
    },
    change() { 
      return (this.amountReceived - this.totalWithDiscount).toFixed(2);
    }
  },
  watch: {
    perPage() {
      this.currentPage = 1;
    }
  },
  mounted() {
    this.fetchArticles();
  },
  methods: {
    GetLitValue(filds, Value) {
      console.log("GetLitValue", Value);
      this.selectedContact = Value;
    },
    async fetchArticles() {
      try {
        const response = await this.$axios.get('/Concept/GetAll');
        this.articles = response.data.data;
      } catch (error) {
        console.error("Error fetching articles:", error);
      }
    },
    addItemToTicket(item) {
      const existingItem = this.ticket.find(t => t.id === item.id);
      if (existingItem) {
        existingItem.quantity++;
      } else {
        this.ticket.push({
          id: item.id,
          description: item.description,
          price: item.priceSale,
          quantity: 1
        });
      }
      this.updateTotal();
    },
    removeItem(index) {
      this.ticket.splice(index, 1);
      this.updateTotal();
    },
    updateTotal() {
      this.total = this.ticket.reduce((sum, item) => sum + item.price * item.quantity, 0).toFixed(2);
    },
    printTicket() {
      const ticketWindow = window.open('', 'PRINT', 'height=600,width=800');
      ticketWindow.document.write('<html><head><title>Ticket</title>');
      ticketWindow.document.write('<style>body { font-family: monospace; }</style>');
      ticketWindow.document.write('</head><body>');
      ticketWindow.document.write('<h2>Ticket de Compra</h2>');
      ticketWindow.document.write('<p>Cliente: ' + (this.selectedContact ? this.selectedContact.name : 'No seleccionado') + '</p>');
      ticketWindow.document.write('<hr>');
      this.ticket.forEach(item => {
        ticketWindow.document.write('<p>' + item.description + ' - ' + item.quantity + ' x $' + item.price + '</p>');
      });
      ticketWindow.document.write('<hr>');
      ticketWindow.document.write('<p>Descuento: ' + this.discount + '%</p>');
      ticketWindow.document.write('<p>Total: $' + this.totalWithDiscount + '</p>');
      ticketWindow.document.write('</body></html>');
      ticketWindow.document.close();
      ticketWindow.focus();
      setTimeout(() => {
        ticketWindow.print();
        ticketWindow.close();
      }, 1000);
    },
    addDenomination(value) {
      var total =
        parseFloat(this.amountReceived) + parseFloat(value);
      if (!isNaN(total)) {

        this.amountReceived = total.toFixed(2);;
      }
    },
    addRow() {
      let newRow = {
        id: null,
        name: '',
        value: 0,
        quantity: 0,
        total: 0
      };
      this.form.denominations.push(newRow);
    },
    removeRow(index) {
      this.form.denominations.splice(index, 1);
      this.calculateTotal();
    },
    calculateTotal() {
      this.totalToSubtract = 0;
      this.form.denominations.forEach(denomination => {
        denomination.total = denomination.value * denomination.quantity;
        this.totalToSubtract += denomination.total;
      });
    },

  }
};
</script>