$(document).foundation();


function isFunction(functionToCheck) {
    var getType = {};
    return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
}


//form pattern

Foundation.Abide.defaults.patterns['CPF'] = /^([0-9]{3}\.?[0-9]{3}\.?[0-9]{3}\-?[0-9]{2})$/;
Foundation.Abide.defaults.patterns['CNPJ'] = /^([0-9]{2}\.?[0-9]{3}\.?[0-9]{3}\/?[0-9]{4}\-?[0-9]{2})$/;
Foundation.Abide.defaults.patterns['CEP'] = /^(\d{5}-?\d{3})$/;
Foundation.Abide.defaults.patterns['telefone'] = /^(\([0-9]{3}\) [0-9]{4,5}-[0-9]{4})$/;
Foundation.Abide.defaults.patterns['DataNascimento'] = /^[0-9]{2}\/[0-9]{2}\/[0-9]{4}$/

if (typeof tinymce !== 'undefined' && tinymce.init)
    tinymce.init({
        selector: '.tynymce',
        height: 500,
        plugins: [
          'advlist autolink lists link image charmap print preview anchor',
          'searchreplace visualblocks code fullscreen',
          'insertdatetime media table contextmenu paste code'
        ],
        toolbar: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
        content_css: [
          '//fast.fonts.net/cssapi/e6dc9b99-64fe-4292-ad98-6974f93cd2a2.css',
          '//www.tinymce.com/css/codepen.min.css'
        ]
    });







/**
 * Iniciação dos modulos
 */

!function () {

    var source = {
        // Only show 10 results at once
        limit: 10,

        // Function to fetch result list and then find a result;
        source: function (query, sync, async) {
            query = query.toLowerCase();
            if (query.length > 3)
                $.getJSON(_ROOT + '/Busca/search?q=' + query, function (data, status) {
                    async(data.filter(function (elem, i, arr) {
                        var name = elem.Nome.toLowerCase();
                        var terms = [name, name.replace('-', '')].concat(elem.Categorias || []);
                        for (var i in terms) if (terms[i].indexOf(query) > -1) return true;
                        return false;
                    }));
                });
        },

        // Name to use for the search result itself
        display: function (item) {
            return item.Nome;
        },

        templates: {
            // HTML that renders if there are no results
            notFound: function (query) {
                return '<div class="tt-empty">No results for "' + query.query + '".</div>';
            },
            // HTML that renders for each result in the list
            suggestion: function (item) {
                if (!item.IsPatrocinado)
                    return '<div><img src="' + item.UrlImagem + '" /><span class="name">' + item.Nome + '</span> <span class="desc">' + item.Resumo + '</span></div>';
                else
                    return '<div><span class="name">' + item.Nome + '</span> <span class="desc">' + item.Resumo + '</span></div>';
            }
        }
    }

    // Search
    $('[data-docs-search]')
      .typeahead({ highlight: false, maxwords: 5 }, source)
      .on('typeahead:select', function (e, sel) {
          window.location.href = sel.Link;
      });

    // Auto-highlight unless it's a phone
    if (!navigator.userAgent.match(/(iP(hone|ad|od)|Android)/)) {
        $('[data-docs-search]').focus();
    }

    $('input[patternType="RG"]').inputmask("99.999.999-9");  //static mask
    $('input[patternType="CPF"]').inputmask("999.999.999-99");  //static mask
    $('input[patternType="CNPJ"]').inputmask("99.999.999/9999-99");  //static mask
    $('input[patternType="CEP"]').inputmask("99999-999");  //static mask
    $('input[patternType="telefone"]').inputmask("(999) 99999-9999");  //static mask
    $('input[patternType="Data"]').inputmask("99/99/9999 99:99:99");  //static mask 
    $('input[patternType="DataNascimento"]').inputmask("99/99/9999");  //static mask 

    $('button.close-button').click(function (e) {
        e.preventDefault();
        var source = {

            // Objeto Enviado para no post
            data: { "idImagem": $(this).data("idimagem") },

            // Url do post
            url: _ROOT + "/Receitas/DeleteImagem",

            // Evento Sucesso
            success: function (retorno, textStatus, XMLHttpRequest) {
                if (!retorno.erro)
                    $this.parent().remove("");

                alert(retorno.mensagem);
            },

            // Evento Erro
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('Error: ' + textStatus + " msg: " + errorThrown);
            }
        };

        $(this).ajaxPost(source);
    });

}();


/**
* ajaxPost module.
* @module ajaxPost
* @requires jQuery
*/

(function ($) {
    "use strict";
    $.ajaxPost = $.fn.ajaxPost = function (options) {

        //if (!(this instanceof ajaxPost))
        //    return new ajaxPost(options);

        var defaults = {

            // Objeto Enviado para no post
            data: {},

            // Url do post
            url: "",

            // Evento Sucesso
            success: function (retorno, textStatus, XMLHttpRequest) {
                alert(retorno.mensagem);
            },

            // Evento Erro
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('Error: ' + textStatus + " msg: " + errorThrown);
            }
        };

        var settings = $.extend({}, defaults, options);

        $.ajax({
            url: settings.url,
            type: "POST",
            data: JSON.stringify(settings.data),
            dataType: "json",
            contentType: 'application/json',
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                settings.error(XMLHttpRequest, textStatus, errorThrown);
            },

            success: function (data, textStatus, XMLHttpRequest) {
                settings.success(data, textStatus, XMLHttpRequest);
            }
        });

        return this;
    };



    //ajaxPost.fn = ajaxPost.prototype = {
    //    post: function () {

    //    }
    //}

    //window.ajaxPost = ajaxPost, $.fn = ajaxPost;

})(jQuery);

/**
* chartPadrao module.
* @module chartPadrao
* @requires jQuery
* @requires kendoChart
* @requires ajaxPost
*/

//(function ($, kendoChart, ajaxPost) {
//    "use strict";
//    $.fn.chartPadrao = function (options) {

//        var defaults = {
//            // Titulo do grafico
//            Titulo: "",

//            // Objeto Enviado para no post
//            dataPost: {},

//            // Url do post
//            url: "",

//            // Evento Sucesso
//            success: function (retorno, textStatus, XMLHttpRequest) {
//                alert(retorno.mensagem);
//            },

//            // Evento Erro
//            error: function (XMLHttpRequest, textStatus, errorThrown) {
//                alert('Error: ' + textStatus + " msg: " + errorThrown);
//            }
//        };

//       return this
//    }

//})(jQuery, kendoChart, ajaxPost);