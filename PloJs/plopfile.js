const config = plop => {
    // controller generator
    plop.setGenerator('controller', {
        description: 'application controller logic',
        prompts: [{
            type: 'input',
            name: 'name',
            message: 'controller name please'
        }],
        actions: [{
                type: 'add',
                path: 'src/{{name}}/{{name}}Controller.cs',
                templateFile: 'plop-templates/controller.hbs'
            },
            {
                type: 'add',
                path: 'src/{{name}}/{{name}}.cs',
                templateFile: 'plop-templates/Entity.hbs'
            },
            {
                type: 'add',
                path: 'src/{{name}}/{{name}}/Index.cshtml',
                templateFile: 'plop-templates/Index.hbs'
            },
            {
                type: 'add',
                path: 'src/{{name}}/{{name}}/Detallis/{{name}}/Index.cshtml',
                templateFile: 'plop-templates/Index2.hbs'
            },
            {
                type: 'add',
                path: 'src/{{name}}/{{name}}/Detallis/{{name}}/{{name}}Controller.cs',
                templateFile: 'plop-templates/controller2.hbs'
            },
            {
                type: 'add',
                path: 'src/{{name}}/{{name}}/Detallis/{{name}}/{{name}}.cs',
                templateFile: 'plop-templates/Entity2.hbs'

            }
        ]
    });
}

module.exports = config;