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
                  <h4 class="card-title"> Orden Total: <span class="bg-warring">{{invoice_total}} </span></h4>   
                  
                </b-form-group>
              </div>
</div>
    </div>
    <div class="row">
    </div>
    <b-modal id="modal-1" title="BootstrapVue">
        <img v-if="SelectItemsId.link" :src="SelectItemsId.link" alt class="avatar-lg rounded-circle img-thumbnail" />
    <p class="my-4"> <h5 class="font-size-16 mb-1">{{ SelectItemsId.description }} - {{ SetTotal(SelectItemsId.priceSale) }} </h5></p>
   <numeric-keypad></numeric-keypad>
   
    <p class="my-4">Cantidad:  <b-form-input   >
                  </b-form-input>
                </p>  
</b-modal>
  </div>
</template>
<script>
var numbro = require("numbro");
var moment = require("moment");
export default {
  data() {
    return {
      SelectionCatalogueId: "e001e621-26f8-4124-8f11-0ce30a140cc0",
      SelectItemsId:"",
      number: "",
      invoice_total:0,
      Shoppingcart:[],
    maxLength: 6,
    showKeypad: true,
      CatalogueList: [],
      Items: [
        {
          id: "a67200e1-190f-478b-88af-60ea9a7aa060",
          description: "Coca-cola",
          catalogueId: "e001e621-26f8-4124-8f11-0ce30a140cc0",
          priceSale: 150,
          link: "https://cnnespanol.cnn.com/wp-content/uploads/2022/06/osos-polares-2.jpg?resize=1536,864",
          stock: 0,
        },
        {
          id: "6c498eba-3234-4293-9b9a-4ac4d14ed50a",
          description: "Pepito",
          catalogueId: "e001e621-26f8-4124-8f11-0ce30a139cc0",
          priceSale: 250,
          link: "https://i.ytimg.com/vi/8LCWXrDbSiw/maxresdefault.jpg",
          stock: 0,
        },
        {
          id: "5a1e1032-f189-4e87-b86c-5d8ed88d2e15",
          description: "Polar",
          catalogueId: "e001e621-26f8-4124-8f11-0ce30a125cc0",
          priceSale: 250,
          link: "data:image/jpeg;base64,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jWY5oc0HmJIgc8zHgru79nTXGWXNdp5agx8egChj2fV2gBt42AZzRLepzFSOe6iil6+Hc2rLxoXTA2Bn1B22g/D4gKJUqsGxBGnYRMEjxwPJWL+w16Yi7pYx+rIx8CvNvYG7z/iqMkRJpEkDMwZBz+C86cns9iasNxu6BcXBoOPtkk4+d1nbaoCXHAkk+S6a/wBk9d57982OgouI/wD0CquN+yW5pNDraqLh2dTXAUnHpolxaeeCQpcdJj5ns3r+Cj4bxBjdyrE8WpePpHx3E7HzWRveDXtIn3ltcMjcmm/TzyHAaT8DyVU65cJBJB6HB+Kr34VbW3tZrzJiNabitxGmZgn1B38lArXreqyv0s9VLt7O5q/q6Nd/+im9/wDSF7XiVgnmTKdxS8BpkAr9G9lqRbZ2rTu23og8shjeS4d2c9m97cvb76m63oyC51QaX6eYYw97UepAAn4L9CU2AAACAAAB0A2VqlIpGoVc2WbzuXoiIu0IiIgIiICIiAiIgIiICIiAiIgIiICIiAiIgIiIC83UmndoPmAV6Ig8m0WjZoHwC9URAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREBERAREQEREH/9k=",
          stock: 0,
        },
      ],
      SelectItems: [],
    };
  },
  created(){
    this.GetProduct();
    this.GetCatalogue();
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
        this.SelectItemsId = Item;
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
