<template>
  <div>
    <div>

      <template v-if="item.showForm === true ">
        <b-form-group v-if="item.type === 0">
          <div >
            <template v-if="item.isRequired">
               <span v-if="labelShow" style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>

              <validation-provider rules="required" v-slot="{ errors }">



                <b-form-input
                  v-model="Scheme[item.field]"
                  type="text"
                  size="sm"

                  autocomplete="off"
                  :name="item.field"
                ></b-form-input>

                <label class="req">*</label>
                <span class="req">{{ errors[0] }}</span>
              </validation-provider>
            </template>

            <template v-else>
              <span   v-if="labelShow"  style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>
              <b-form-input
                v-model="Scheme[item.field]"
                type="text"
                size="sm"
                :readonly="item.ReadOnly"
                autocomplete="off"
                :name="item.field"
              ></b-form-input>

            </template>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type === 1" >
          <div >
             <span v-if="labelShow" style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>

            <vSelect
              :field="item"
              @CustomChange="GetLitValue"
              :select="Scheme[item.field]"
              style="width: 250px;"
              size="sm"
             
            >
            </vSelect>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type === 2">
          <div >
            <template v-if="item.isRequired">
              <span    v-if="labelShow" style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>
              <validation-provider rules="required" v-slot="{ errors }">

                <b-form-input
                  v-model="Scheme[item.field]"
                  autocomplete="off"
                  size="sm"
                  type="text"
                  class="form-control"
                  :name="item.field"
                ></b-form-input>

                <label class="req">*</label>
                <span class="req">{{ errors[0] }}</span>
              </validation-provider>
            </template>

            <template v-else>
              <span  v-if="labelShow"  style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>
              <b-form-input
                v-model="Scheme[item.field]"
                autocomplete="off"
                :name="item.field"
                size="sm"
              ></b-form-input>





            </template>
          </div>
        </b-form-group>

        <b-form-group v-if="item.type === 3">
          <div >
             <span v-if="labelShow" style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>
            <input type="checkbox" id="checkbox" v-model="Scheme[item.field]" />
          </div>
        </b-form-group>
        <b-form-group v-if="item.type === 4">
          <div >
             <span v-if="labelShow" style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>
            <b-form-datepicker
              v-model="Scheme[item.field]"
              :name="item.field"
              locale="es"
              size="sm"
              :disabled="$route.query.Action == 'show'"
              class="mb-2"
            ></b-form-datepicker>
          </div>
        </b-form-group>

        <b-form-group v-if="item.type === 5">
          <div >
             <span v-if="labelShow" style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>
            <b-form-textarea
              v-model="Scheme[item.field]"
              rows="3"
              size="sm"
              max-rows="6"
              :name="item.field"
            ></b-form-textarea>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type === 6">
          <div >
             <span v-if="labelShow" style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>

            <template v-if="item.isRequired">
              <validation-provider rules="required" v-slot="{ errors }">
                <b-form-input
                  size="sm"
                  v-model="Scheme[item.field]"
                  autocomplete="off"
                  :name="item.field"
                  type="password"
                  class="form-control"
                ></b-form-input>

                <label class="req">*</label>
                <span class="req">{{ errors[0] }}</span>
              </validation-provider>
            </template>

            <template v-else>
              <b-form-input
                size="sm"
                v-model="Scheme[item.field]"
                autocomplete="off"
                :name="item.field"
                type="password"

              ></b-form-input>
            </template>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type === 7">
          <div class="col-6" >
        

            <template  >
              <span   v-if="labelShow" style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>
              <b-form-input
                size="sm"
                v-model="Scheme[item.field]"
                type="text"
                w-100
                style="width: 100px;"
                readonly
                :name="item.field"
              ></b-form-input>

            </template>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type === 8">
          <div class="col-lg-12 col-md-12 col-sm-12">
            <template v-if="item.isRequired">
               <span v-if="labelShow" style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>
              <validation-provider rules="required" v-slot="{ errors }">
                <ckeditor
                  v-model="Scheme[item.field]"
                  :editor="editor"
                  :name="item.field"
                ></ckeditor>

                <label class="req">*</label>
                <span class="req">{{ errors[0] }}</span>
              </validation-provider>
            </template>

            <template v-else>
              <span   v-if="labelShow" style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>
              <ckeditor
                v-model="Scheme[item.field]"
                :editor="editor"
                :name="item.field"
              ></ckeditor>
              ---
              <div v-html="Scheme[item.field]"></div>
            </template>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type === 9">
          <div class="col-5">
             <span v-if="labelShow" style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}  </span>
 
            <b-form-input
              v-model="Scheme[item.field]"
              type="date"
              size="sm"
              w-100 
              style="width: 140px;"
               
             
              
              :name="item.field"
            ></b-form-input>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type === 10">
          <div    >
             <span v-if="labelShow" style="font-size:14px ; font-family: Georgia, 'Times New Roman', Times, serif; font:bold" >{{ item.label }}</span>

            <vSelectContact
              :field="item"
              @CustomChange="GetLitValue"
              :select="Scheme[item.field]"
              w-100
              style="width: 200px;"

            >
            </vSelectContact>
          </div>
        </b-form-group>
      </template>
    </div>
  </div>
</template>

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
    list: [],
    Scheme: {},
    editor: ClassicEditor,
  }),

  props: ["item", "FieldsData", "labelShow"],
  watch: {
    Scheme() {
      this.$emit("CustomChange", this.Scheme);
    },
  },

  created() {
    this.Scheme = this.FieldsData;
    if (this.Scheme[this.item.field] === undefined) {
      this.SetValues();
    } else {
      if (this.item.type === 9) {
        const now = new Date();
        var month = now.getMonth() + 1; // Los meses en JavaScript comienzan en cero, por lo que debes sumar 1.
        var day = now.getDate();
        var year = now.getFullYear();
        var formattedDate = `${month.toString().padStart(2, '0')}/${day.toString().padStart(2, '0')}/${year}`;
        if (this.Scheme[this.item.field] === null) {
          this.Scheme[this.item.field] = new Date(formattedDate).toISOString().substr(0, 10);
        }
        else {
          this.Scheme[this.item.field] = this.Scheme[this.item.field].substr(0, 10);
        }
 
      }
    }
  },
  methods: {
    SetValues() {
      this.Scheme[this.item.field] = this.item.defaultValue;

      if (this.item.type === 9) {
        const now = new Date();
        var month = now.getMonth() + 1; // Los meses en JavaScript comienzan en cero, por lo que debes sumar 1.
        var day = now.getDate();
        var year = now.getFullYear();
        var formattedDate = `${month.toString().padStart(2, '0')}/${day.toString().padStart(2, '0')}/${year}`;
        this.Scheme[this.item.field] = new Date(formattedDate).toISOString().substr(0, 10);



      }
      if (this.item.type === 3) {

          switch (this.item.defaultValue) {
          case "true":
          case "yes":
          case "1":
            this.Scheme[this.item.field] = true;

          case "false":
          case "no":
          case "0":
          case null:
          case undefined:
            this.Scheme[this.item.field] = false;

          default:
            this.Scheme[this.item.field] = false;
        }

      }
    },
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
