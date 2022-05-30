<script>
/**
 * Starter page
 */
export default {
    head() {
        return {
            title: `${this.title} | `
        };
    },
    data() {
        return {
            title: "Bienvenido",
            items: [ 
                {
                    text: " ",
                    active: true
                }
            ]
        };
    },
    middleware: "authentication"
};
</script>

<template>
<div>
    <PageHeader :title="title" :items="items" />
 
</div>
</template>
