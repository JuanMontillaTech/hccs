<template>
  <div>
    <template v-if="item.showForm && item.isActive">
 
 
  
    <tr>
      <th>{{ item.label }}    <span class="req" v-if="item.isRequired">*</span> </th>
      
       
      </tr>  
     <tr>
      <td  >
        
        <b-form-group 
           
          :label-for="`input-${item.type}-${item.field}`" 
          label-cols-sm="4" 
          v-if="isAllowedType(item.type)" 
        >
     
          <b-form-input
            :id="`input-${item.type}-${item.field}`"
            v-model="Scheme[item.field]"
            :type="getAllowedType(item.type).inputType" 
            size="sm"
            autocomplete="off"
            :name="item.field"
             style="width: 250px;"
            :readonly="item.type === 7" 
          />
         
        </b-form-group>
        
        <b-form-group v-if="item.type === 1"  label-for="select-1" label-cols-sm="4">
          <vSelect
          
             :id="`select-${item.type}-${item.field}`"
            :field="item"
            @CustomChange="GetLitValue"
            :select="Scheme[item.field]"
            style="width: 250px;"
            size="sm"
          />
        </b-form-group>
  
        <b-form-group v-if="item.type === 10"  label-for="select-10" label-cols-sm="4">
          <vSelectContact
            id="select-10"
            :field="item"
            @CustomChange="GetLitValue"
            :select="Scheme[item.field]"
          />
        </b-form-group>
  
        <b-form-group v-if="item.type === 3"  label-cols-sm="4">
          <b-form-checkbox 
            v-model="Scheme[item.field]" 
            :name="item.field" 
              :id="`checkbox-${item.type}-${item.field}`"
            switch
          /> 
        </b-form-group>
  
        <b-form-group v-if="item.type === 4"  label-for="datepicker-4" label-cols-sm="4">
          <b-form-datepicker
          
             :id="`datepicker-${item.type}-${item.field}`"
            v-model="Scheme[item.field]"
            :name="item.field"
            locale="es"
            size="sm"
            :disabled="$route.query.Action == 'show'"
          />
        </b-form-group>
  
        <b-form-group v-if="item.type === 5"  label-for="textarea-5" label-cols-sm="4">
          <b-form-textarea 
            :id="`textarea-${item.type}-${item.field}`"
            v-model="Scheme[item.field]"
            rows="3"
            size="sm"
            max-rows="6"
            :name="item.field"
          />
        </b-form-group>
  
        <b-form-group v-if="item.type === 8"  label-cols-sm="4"> 
          <ckeditor
            v-model="Scheme[item.field]"
            :editor="editor"
            :name="item.field"
          />
          <div v-html="Scheme[item.field]"></div>
        </b-form-group>
        <b-form-group 
        v-if="item.type === 11"  label-cols-sm="4">  
          <select   :id="`input-${item.type}-${item.field}`" v-model="Scheme[item.field]" class="form-control form-control-sm">
            <option v-for="year in years" :key="year" :value="year">{{ year }}</option>
          </select>
        </b-form-group>
      </td>
     </tr>
    </template>
    
  </div>
</template>

<script>
// ... (el script se mantiene igual) ...
</script>

<script>
import { ValidationProvider, extend } from "vee-validate";
import { required } from "vee-validate/dist/rules";
import CKEditor from "@ckeditor/ckeditor5-vue";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

extend("required", {
  ...required,
  message: "El campo es requerido",
});

export default {
  name: "InfiniteScroll",
  components: {
    ValidationProvider,
    ckeditor: CKEditor.component,
  },
  data: () => ({
    Scheme: {},
    editor: ClassicEditor,
    years: null,
    allowedTypes: [
      { type: 0, inputType: 'text' },
      { type: 2, inputType: 'text' },
      { type: 6, inputType: 'password' },
      { type: 7, inputType: 'text' },
      { type: 9, inputType: 'date' }, 
    ],
  }),

  props: ["item", "FieldsData", "labelShow"],
  watch: {
    Scheme: {
      handler() {
        this.$emit("CustomChange", this.Scheme);
      },
      deep: true, // Observar cambios profundos en el objeto Scheme
    }
  },

  created() {
    this.Scheme = this.FieldsData;
    if (this.Scheme[this.item.field] === undefined) {
      this.SetValues();
    } else {
      if (this.item.type === 9 && this.Scheme[this.item.field]) {
        this.Scheme[this.item.field] = this.Scheme[this.item.field].substr(0, 10);
      }
      if (this.item.type === 11) {
       
       this.years= this.generateYears(2020, new Date().getFullYear() + 5);
        
     }
    }
   
  },
  methods: {
  
    isAllowedType(type) {
    
      return this.allowedTypes.some(item => item.type === type);
    },
    getAllowedType(type) { 
      return this.allowedTypes.find(item => item.type === type);
    }, 
    SetValues() {
      this.Scheme[this.item.field] = this.item.defaultValue;

      if (this.item.type === 9) {
        const now = new Date();
        this.Scheme[this.item.field] = now.toISOString().substr(0, 10);  
      }
      if (this.item.type === 11) {
       
        this.years= this.generateYears(2020, new Date().getFullYear() + 5);
         
      }
      if (this.item.type === 3) {
        this.Scheme[this.item.field] = ["true", "yes", "1"].includes(this.item.defaultValue);
      }
      
    },
    GetLitValue(filds, Value) {
      this.Scheme[filds] = Value;
      
    },
    generateYears(startYear, endYear) {
      const years = [];
      for (let year = startYear; year <= endYear; year++) {
        years.push(year);
      }
      return years;
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