<template>
 

  <div>
      <List     Title="Facturas"
    TransactionsType="1"
    Form="factura"
    IsClient="true"
    DateLabel="Fecha de Factura"
    :Path="$route.path"
     DocumentTypeId="1"></List>

  
  </div>
</template>
