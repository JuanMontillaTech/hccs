<template>
  <div>
    <div>
      <template v-if="item.showForm == 1">
        <b-form-group v-if="item.type == 0">
          <div class="col-4">
            <template v-if="item.isRequired">
              <h4 class="card-title" v-if="labelShow">{{ item.label }}</h4>
              <validation-provider rules="required" v-slot="{ errors }">
                <input
                  v-model="Scheme[item.field]"
                  name="Scheme[item.field]"
                  type="text"
                  autocomplete="off"
                  class="form-control"
                />
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
                autocomplete="off"
              />
            </template>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type == 1">
          <div class="col-6">
            <h4 class="card-title" v-if="labelShow">{{ item.label }}</h4>
            <vSelect
              :field="item"
              @CustomChange="GetLitValue"
              :select="Scheme[item.field]"
            >
            </vSelect>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type == 2">
          <div class="col-4">
            <template v-if="item.isRequired">
              <h4 class="card-title">{{ item.label }}</h4>
              <validation-provider rules="required" v-slot="{ errors }">
                <input
                  v-model="Scheme[item.field]"
                  autocomplete="off"
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
              ></b-form-input>
            </template>
          </div>
        </b-form-group>

        <b-form-group v-if="item.type == 3">
          <div class="col-4">
            <h4 class="card-title" v-if="labelShow">{{ item.label }}</h4>
            <input type="checkbox" id="checkbox" v-model="Scheme[item.field]" />
          </div>
        </b-form-group>
        <b-form-group v-if="item.type == 4">
          <div class="col-4">
            <h4 class="card-title" v-if="labelShow">{{ item.label }}</h4>
            <b-form-datepicker
              v-model="Scheme[item.field]"
              locale="es"
              :disabled="$route.query.Action == 'show'"
              class="mb-2"
            ></b-form-datepicker>
          </div>
        </b-form-group>

        <b-form-group v-if="item.type == 5">
          <div class="col-4">
            <h4 class="card-title" v-if="labelShow">{{ item.label }}</h4>
            <b-form-textarea
              v-model="Scheme[item.field]"
              rows="3"
              max-rows="6"
            ></b-form-textarea>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type == 6">
          <div class="col-4">
            <h4 class="card-title" v-if="labelShow">{{ item.label }}</h4>

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
                class="form-control"
              ></b-form-input>
            </template>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type == 7">
          <div class="col-4">
            <template v-if="item.isRequired">
              <h4 class="card-title" v-if="labelShow">{{ item.label }}</h4>
              <validation-provider rules="required" v-slot="{ errors }">
                <input
                  v-model="Scheme[item.field]"
                  name="myinput"
                  type="text"
                />
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
                readonly
              />
            </template>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type == 8">
          <div class="col-lg-12 col-md-12 col-sm-12">
            <template v-if="item.isRequired">
              <h4 class="card-title" v-if="labelShow">{{ item.label }}</h4>
              <validation-provider rules="required" v-slot="{ errors }">
                <ckeditor
                  v-model="Scheme[item.field]"
                  :editor="editor"
                ></ckeditor>

                <label class="req">*</label>
                <span class="req">{{ errors[0] }}</span>
              </validation-provider>
            </template>

            <template v-else>
              <h4 class="card-title">{{ item.label }}</h4>
              <ckeditor
                v-model="Scheme[item.field]"
                :editor="editor"
              ></ckeditor>
              ---
              <div v-html="Scheme[item.field]"></div>
            </template>
          </div>
        </b-form-group>
        <b-form-group v-if="item.type == 9">
          <div class="col-5">
            <h4 class="card-title" v-if="labelShow">{{ item.label }}</h4>

            <b-form-input
              v-model="Scheme[item.field]"
              type="date"
            ></b-form-input>
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

    if (this.Scheme[this.item.field] == undefined) {
      this.Scheme[this.item.field] = this.item.defaultValue;
      if (this.item.type == 9) {
        this.Scheme[this.item.field] = new Date().toISOString().substr(0, 10);
      }
    }
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
