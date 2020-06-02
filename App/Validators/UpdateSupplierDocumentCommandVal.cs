using FluentValidation;
using GODPAPIs.Contracts.Commands.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Validations
{
    public class UpdateSupplierDocumentCommandVal: AbstractValidator<UpdateSupplierDocumentCommand>
    {
        public UpdateSupplierDocumentCommandVal()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Document).NotEmpty();
        }
    }
}
