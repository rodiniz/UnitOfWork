namespace UnitOfWork
{
    using System;
    using System.Threading.Tasks;
    using Arch.EntityFrameworkCore.UnitOfWork;
    using FluentValidation;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseCrudController<M, T> : ControllerBase where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IRepository<T> _repository;

        public readonly IAdapter<M, T> _adapter;

        public readonly IValidator<M> _validator;


        public BaseCrudController(IUnitOfWork unitOfWork, IAdapter<M, T> adapter, IValidator<M> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<T>();
            _adapter = adapter;
            _validator = validator;
        }

        [Authorize("Bearer")]
        [HttpGet]
        [Route("Get")]
        public virtual async Task<IActionResult> GetAsync(int id)
        {
            return Ok(new 
            {
                Success = true,
                Data = await _repository.FindAsync(id)
            });
        }

        [Authorize("Bearer")]
        [Route("update")]
        [HttpPut]
        public virtual async Task<IActionResult> PutAsync(M model)
        {
            try
            {
                _validator.ValidateAndThrow(model);
                var entity = _adapter.convertFromModel(model);
                _repository.Update(entity);

                await _unitOfWork.SaveChangesAsync();
                return Ok(new
                {
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    ex.Message
                });
            }
          
        }

        [Authorize("Bearer")]
        [Route("create")]
        [HttpPost]
        public virtual async Task<IActionResult> PostAsync([FromBody] M model)
        {
            try
            {
                _validator.ValidateAndThrow(model);
               var entity = _adapter.convertFromModel(model);

               await _repository.InsertAsync(entity);

               await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return Ok(new 
                {
                    Success = false,
                    ex.Message
                });
            }

            return Ok(new 
            {
                Success = true,
                Data = model
            });
        }

        /// <summary>
        /// Apaga a entidade do banco de dados.
        /// </summary>
        /// <param name="entidade">A entidade que será apagada</param>
        /// <returns></returns>
        [Authorize("Bearer")]
        [HttpDelete]
        public virtual async Task<IActionResult> DeleteAsync(T entidade)
        {
            _repository.Delete(entidade);
            await _unitOfWork.SaveChangesAsync();
            return Ok(new 
            {
                Success = true
            });
        }
    }
}
