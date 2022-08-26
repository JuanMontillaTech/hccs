<template>
  <div>
 
    <div v-for="(item, ind) in GetFilterColum()" :key="ind" >
      <template v-if="item.showForm == 1">
        <b-form-group v-if="item.type == 0">
          <h4 class="card-title">{{ item.label }}</h4>
          <b-form-input
            v-model="principalSchema[item.field]"
            size="sm"
            trim
            type="text"
          >
          </b-form-input>
        </b-form-group>
        <b-form-group v-if="item.type == 1">
          <h4 class="card-title">{{ item.label }}</h4>
          <vSelect
            :field="item"
            @CustomChange="GetLitValue"
            :select="principalSchema[item.field]"
          >
          </vSelect>
        </b-form-group>
        <b-form-group v-if="item.type == 2">
          <h4 class="card-title">{{ item.label }}</h4>

          <b-form-input
            v-model="principalSchema[item.field]"
            autocomplete="off"
            v-mask="'###,###,###,###,###.##'"
          ></b-form-input>
        </b-form-group>

        <b-form-group v-if="item.type == 3">
          <h4 class="card-title">{{ item.label }}</h4>
          <input
            type="checkbox"
            id="checkbox"
            v-model="principalSchema[item.field]"
          />
        </b-form-group>
        <b-form-group v-if="item.type == 4">
          <h4 class="card-title">{{ item.label }}</h4>
          <b-form-datepicker
            v-model="principalSchema[item.field]"
            locale="es"
            :disabled="$route.query.Action == 'show'"
            class="mb-2"
          ></b-form-datepicker>
        </b-form-group>
        <b-form-group v-if="item.type == 5">
          <h4 class="card-title">{{ item.label }}</h4>
          <b-form-textarea
            v-model="principalSchema[item.field]"
            rows="3"
            max-rows="6"
           
          ></b-form-textarea>
        </b-form-group>
             <b-form-group v-if="item.type == 6">
          <h4 class="card-title">{{ item.label }}</h4>
          <b-form-input
            v-model="principalSchema[item.field]"
            rows="3"
            max-rows="6"
           type="password"
          ></b-form-input>
        </b-form-group>
      </template>
    </div>
  </div>
</template>

<script>
export default {
  name: "InfiniteScroll",
  data: () => ({
    list: [], 
    principalSchema: {},
  }),
  props: ["fields", "col", "FildsData"], 
  watch:{
        principalSchema() {          
          this.setSelected(this.principalSchema);    
    }
  },
  created(){
    console.log("Data",this.FildsData )
    this.principalSchema= this.FildsData;

  },
  methods: {
      GetFilterColum() {
      let results = this.fields.filter((field) => field.columnIndex == this.col);
      return results;
    },
    setSelected(fromElement) {
      this.$emit("CustomChange", fromElement);
    }, 
     GetLitValue(filds, Value) {
      this.principalSchema[filds] = Value;
    },
  },
};
</script>

<style scoped>
.loader {
  text-align: center;
  color: #bbbbbb;
}
</style>
