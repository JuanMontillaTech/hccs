<template>
<div>
<!-- <ListDynamicExpress> </ListDynamicExpress> -->
<Datatable   :customLinks="customElements"  :includeNewOption="true"/>
</div>
</template>
<script>

export default {


  data() {
    return {
      customElements: [   ],

    };
  },
}
</script>




