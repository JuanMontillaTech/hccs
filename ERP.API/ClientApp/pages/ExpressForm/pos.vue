<template>
  <div>
    <div class="row">
      <div class="col-8">

        <table>
          <thead>
            <tr>
              <th v-for="rowCata in CatalogueList" :key="rowCata.id">
                <button v-if="rowCata.isActive"
                  class="btn btn-success rounded-circle mb-3 p-10"
                  @click="GetFilterItems(rowCata.id)"
                >
                  {{ rowCata.name }}
                </button>
              </th>
            </tr>
          </thead>
        </table>
        <div class="row">
       
        <div class="col-sm-5" v-for="item in SelectItems" :key="item.id">

          <template   v-if="item.isActive">
         
              <!-- v-b-modal.modal-1  -->
                  <b-button @click="GetItems(item)" class=" bg-white ">  
                    <FileImg :SourceId="item.id"   Setclass="avatar-lg rounded-circle img-thumbnail"> </FileImg>
                      <img v-if="item.link" :src="item.link" alt class="avatar-lg rounded-circle img-thumbnail" />
                          <div class="  mx-auto mb-4" v-if="!item.link">
                              <div class="  text-primary">
                                <h5 class="font-size-16 mb-1">{{ item.description }} - {{ SetTotal(item.priceSale) }} </h5>
                              </div>
                          </div> 
                        </b-button>
                <h5  v-if="item.link" class="font-size-16 mb-1">{{ item.description }} - {{ SetTotal(item.priceSale) }} </h5> 
             
        
          </template>
        </div>
      </div>
      </div>
      <div class="col-4 bg-white" >
        <h4>Orden Total: <span class="bg-warring">{{invoice_total}} </span></h4>   
      
        <vueselect
          :options="Tablelist"
          :reduce="(row) => row.id"
          label="name"
          v-model="Tablelistselect" 
        
        >
    
    
 
        </vueselect>
        <b-form-group> <h4>Cliente</h4>
                      <b-form-input
                        
                        class="mb-2"
                         
                         
                        size="sm"
                      >
                      </b-form-input>
                    </b-form-group>   
        <table>
              <tr v-for="(rowShoppingcart, index) in Shoppingcart" :key="index">
              <th >
                <b-button
                        size="sm"
                        variant="danger"
                        @click="removeRow(index)"
                      >
                        <i class="fas fa-trash"></i>
                      </b-button>  {{rowShoppingcart.description}}   ${{rowShoppingcart.priceSale}}        
              </th>
              <td class="text-center">
                      <span
                        class="fs-2   bg-greenTwo rounded-pill mt-1 ms-2 p-4"
                        >{{rowShoppingcart.count}}</span
                      >
                    </td>
         
              <td>
             
                    <b-button-group class="mt-4 mt-md-0">
                    
                      <b-button size="sm" variant="info"  @click=" addRow(index) ">
                        <i class="fas fa-plus"></i>
                      </b-button>
                    </b-button-group>
                  </td>
            </tr>
         
        </table>
        <div class="col-lg-3">
                <b-form-group>
                  <h2 class="card-title">   Total:  {{invoice_total}} </h2>   
                  
                </b-form-group>
              </div>
</div>
    </div>
   
  </div>
</template>
<script>
var numbro = require("numbro");
var moment = require("moment");
export default {
  data() {
    return {
      invoice_total:0,
      Shoppingcart:[],
      Tablelist:[],
      Tablelistselect:"",  
      CatalogueList: [],
      Items: [],
      SelectItems: [],
    };
  },
  created(){
    this.GetProduct();
    this.GetCatalogue();
    this.GetTablelist();
  },
  methods: {
    GetCatalogue() {
      this.$axios
        .get(`Catalogue/GetAll`)
        .then((response) => {
          this.CatalogueList = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetTablelist() {
      this.$axios
        .get(`TransactionLocation/GetAll`)
        .then((response) => {
          this.Tablelist = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    removeRow(index) {
    
 console.log("remove element",index, this.Shoppingcart[index]);
        var total =  parseFloat(this.invoice_total) -  parseFloat(   this.Shoppingcart[index].priceSale) ;
       this.invoice_total = total;
       let itemFound  =this.Shoppingcart.find(items => items.id == this.Shoppingcart[index].id);
         if(itemFound.count == 1){
        this.Shoppingcart.splice(index, 1);
        }else{
         itemFound.count = itemFound.count - 1;
        }

    },
    addRow(index) {
      let Item = this.Shoppingcart[index];
      var total =  parseFloat(this.invoice_total) +  parseFloat(   Item.priceSale) ;

       this.invoice_total = total;     
       let itemFound  =this.Shoppingcart.find(items => items.id ==Item.id)
       itemFound.count = itemFound.count + 1;
     
         
    },
    GetProduct() {
      this.$axios
        .get(`Concept/GetAll`)
        .then((response) => {
          this.Items = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    onInput(key) {
      this.number = (this.number + key).slice(0, this.maxLength);
    },
    onDelete() {
      this.number = this.number.slice(0, this.number.length - 1);
    },
    onReset() {
      this.number = "";
    },
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    GetItems(Item) {
     
        let itemFound  =this.Shoppingcart.find(items => items.id ==Item.id);
        console.log("dsdf",itemFound)
        if (itemFound == undefined){
        this.Shoppingcart.push({id: Item.id, count:1 , description:Item.description,  priceSale: Item.priceSale})
       }else{
        itemFound.count = itemFound.count + 1;
       }
       var total =  parseFloat(Item.priceSale)  + parseFloat(this.invoice_total);
        this.invoice_total = total;
    },
    
 
    GetFilterItems(SelectionCatalogueId) {
      this.SelectItems = [];
      this.SelectItems = this.Items.filter(
        (element) => element.catalogueId == SelectionCatalogueId
      );
    },
  },
};
</script>
