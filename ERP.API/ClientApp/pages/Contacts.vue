<template>
    <div class="d-flex justify-content-center">
        <div class="w-75 my-5">
            <!-- <vue-good-table
            :columns="columns"
            :rows="rows"
            :search-options="{
            enabled: true
            }"
            :pagination-options="{
            enabled: true,
            mode: 'records'
            }"
            /> -->
            <vue-good-table
            :columns="columns"
            :rows="rows"
            :search-options="{
            enabled: true
            }"
            :pagination-options="{
            enabled: true,
            mode: 'records'
            }"
            >
           <template slot="table-row" slot-scope="props">
      <span v-if="props.column.field == 'action'">
        <b-button variant="warning" @click="editRow(props.row.id)">Edit</b-button>
        <b-button variant="danger" @click="deleteRow(props.row.id)">Delete</b-button>
      </span>
      <span v-else>
        {{props.formattedRow[props.column.field]}}
      </span>
    </template>
            </vue-good-table>


        </div>
    </div>
</template>

<script>
export default {
    name: 'Contacts',
    data(){
    return {
        columns: [
            {
                label: 'Name',
                field: 'name',
            },
            {
                label: 'Age',
                field: 'age',
                type: 'number',
            },
            {
                label: 'Created On',
                field: 'createdAt',
                type: 'date',
                dateInputFormat: 'yyyy-MM-dd',
                dateOutputFormat: 'MMM do yy',
            },
            {
                label: 'Percent',
                field: 'score',
                type: 'percentage',
            },
            {
                label: 'Action',
                field: 'action',
            }
        ],
        rows: [
            { id:1, name:"John", age: 20, createdAt: '2011-10-31',score: 0.03343 },
            { id:2, name:"Jane", age: 24, createdAt: '2011-10-31', score: 0.03343 },
            { id:3, name:"Susan", age: 16, createdAt: '2011-10-30', score: 0.03343 },
            { id:4, name:"Chris", age: 55, createdAt: '2011-10-11', score: 0.03343 },
            { id:5, name:"Dan", age: 40, createdAt: '2011-10-21', score: 0.03343 },
            { id:6, name:"John x", age: 20, createdAt: '2011-10-31', score: 0.03343 },
            { id:7, name:"John z", age: 20, createdAt: '',score: 0.03343 },
            { id:8, name:"Jane v", age: 24, createdAt: '2011-10-31', score: 0.03343 },
            { id:9, name:"Susan a", age: 16, createdAt: '2011-10-30', score: 0.03343 },
            { id:10, name:"Chris s", age: 55, createdAt: '2011-10-11', score: 0.03343 },
            { id:11, name:"Dan z", age: 40, createdAt: '2011-10-21', score: 0.03343 },
            { id:12, name:"John s", age: 20, createdAt: '2011-10-31', score: 0.03343 },
        ],
    };
},
methods: {
    editRow(id) {
        console.log(id, 'EDIT')
    },
    deleteRow(id) {
        console.log(id, 'DELETE')
    }
}
};
</script>
