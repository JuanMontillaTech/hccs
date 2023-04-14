<template>
  <div>
    <DatatableLinkOut
      :customLinks="customElements"
      :PageCreate="PageCreate"
      :PageEdit="PageEdit"
      :includeNewOption="true"/>

  </div>
</template>

<script>

export default {
  data() {
    return {
      customElements: [],
      controller: "Journal",
      PageCreate: "/Accounting/JournalEntryCreateOrUpdate",
      PageEdit: "/Accounting/JournalEntryCreateOrUpdate",
    }
  }
};
</script>
