<template>
  <div class="container">
    <div class="row">
      <div class="col-md-6 offset-md-3">
        <h2 class="text-center">Bank Deposit Form</h2>
        <form @submit.prevent="submitForm">
          <div class="form-group">
            <label for="date">Fecha:</label>
            <input type="date" class="form-control" id="date" v-model="date" required>
          </div>
          <div class="form-group">
            <label for="receivedFrom">Número de recibo:</label>
            <input type="text" class="form-control" id="receivedFrom" v-model="receivedFrom" required>
          </div>
          <div class="form-group">
            <label for="amount">Amount:</label>
            <input type="number" class="form-control" id="amount" v-model="amount" required>
          </div>
          <div class="form-group">
            <label for="paymentMethod">Payment Method:</label>
            <select class="form-control" id="paymentMethod" v-model="paymentMethod" required>
              <option value="Credit Card">Credit Card</option>
              <option value="Wire Transfer">Wire Transfer</option>
              <option value="Deposit">Deposit</option>
              <option value="Cash">Cash</option>
              <option value="Check">Check</option>
            </select>
          </div>
          <div class="form-group">
            <label for="concept">Concept:</label>
            <input type="text" class="form-control" id="concept" v-model="concept">
          </div>
          <button type="submit" class="btn btn-primary btn-block">Register Deposit</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      date: null,
      receivedFrom: '',
      amount: null,
      paymentMethod: 'Cash',
      concept: ''
    };
  },
  methods: {
    submitForm() {
      // Here you can add the logic to send the form data to your backend
      // or perform other necessary actions.
      console.log('Form submitted:', this.formData);
    }
  },
  computed: {
    formData() {
      return {
        date: this.date,
        receivedFrom: this.receivedFrom,
        amount: this.amount,
        paymentMethod: this.paymentMethod,
        concept: this.concept
      };
    }
  }
};
</script>