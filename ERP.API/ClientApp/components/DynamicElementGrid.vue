<template>
  <div>
    <div >
      
      <template v-if="item.showForm == 1">
        <b-form-group v-if="item.type == 0">
        
          <template v-if="item.isRequired">
            <h4 class="card-title"  v-if="labelShow"> {{ item.label }}</h4>
            <validation-provider rules="required" v-slot="{ errors }">
              <input v-model="Scheme[item.field]" name="myinput" type="text" />
              <label class="req">*</label>
              <span class="req">{{ errors[0] }}</span>
            </validation-provider>
          </template>

          <template v-else>
            <h4 class="card-title">{{ item.label }}</h4>
            <input
              v-model="Scheme[item.field]"
              type="text"
              class="form-control"
            />
          </template>
        </b-form-group>
        <b-form-group v-if="item.type == 1">
          <h4 class="card-title"  v-if="labelShow">{{ item.label }}</h4>
          <vSelect
            :field="item"
            @CustomChange="GetLitValue"
            :select="Scheme[item.field]"
          >
          </vSelect>
        </b-form-group>
        <b-form-group v-if="item.type == 2">
          <template v-if="item.isRequired">
            <h4 class="card-title">{{ item.label }}</h4>
            <validation-provider rules="required" v-slot="{ errors }">
              <input
                
                v-model="Scheme[item.field]"
                autocomplete="off"
                v-mask="'###,###,###,###,###.##'"
                type="text"
                class="form-control"
              />
               <label class="req">*</label>
              <span class="req">{{ errors[0] }}</span>
            </validation-provider>
          </template>

          <template v-else>
            <h4 class="card-title">{{ item.label }}</h4>
            <b-form-input
              v-model="Scheme[item.field]"
              autocomplete="off"
              v-mask="'###,###,###,###,###.##'"
            ></b-form-input>
          </template>
        </b-form-group>

        <b-form-group v-if="item.type == 3">
          <h4 class="card-title"  v-if="labelShow">{{ item.label }}</h4>
          <input type="checkbox" id="checkbox" v-model="Scheme[item.field]" />
        </b-form-group>
        <b-form-group v-if="item.type == 4">
          <h4 class="card-title"  v-if="labelShow">{{ item.label }}</h4>
          <b-form-datepicker
            v-model="Scheme[item.field]"
            locale="es"
            :disabled="$route.query.Action == 'show'"
            class="mb-2"
          ></b-form-datepicker>
        </b-form-group>
        <b-form-group v-if="item.type == 5">
          <h4 class="card-title"  v-if="labelShow">class="card-title">{{ item.label }}</h4>
          <b-form-textarea
            v-model="Scheme[item.field]"
            rows="3"
            max-rows="6"
          ></b-form-textarea>
        </b-form-group>
        <b-form-group v-if="item.type == 6">
          <h4 class="card-title"  v-if="labelShow">>{{ item.label }}</h4>

          <template v-if="item.isRequired">
            <validation-provider rules="required" v-slot="{ errors }">
              <input                 
                v-model="Scheme[item.field]"
                autocomplete="off"
                type="password"
                class="form-control"
              />
              <label class="req">*</label>
              <span class="req">{{ errors[0] }}</span>
            </validation-provider>
          </template>

          <template v-else>
            <b-form-input
              v-model="Scheme[item.field]"
              autocomplete="off"
              type="password"
            ></b-form-input>
          </template>
        </b-form-group>
      </template>
    </div>
  </div>
</template>

<script>
import { ValidationProvider, extend } from "vee-validate";
import { required } from "vee-validate/dist/rules";

extend("required", {
  ...required,
  message: "El campo es requerido",
});
 
 
export default {
  name: "InfiniteScroll",
  components: {
    ValidationProvider,
  },
  data: () => ({
    list: [],
    Scheme: {},
    
  }),

  props: ["item", "FieldsData", "labelShow"],
  watch: {
    Scheme() {
     this.$emit("CustomChange", this.Scheme);
    },
  },

  created() {
    this.Scheme = this.FieldsData;
  },
  methods: {
    
    
    GetLitValue(filds, Value) {
      this.Scheme[filds] = Value;
    },
  },
};
</script>

<style scoped>
.loader {
  text-align: center;
  color: #bbbbbb;
}
.req {
  
  color: #ff0202;
}
</style>
