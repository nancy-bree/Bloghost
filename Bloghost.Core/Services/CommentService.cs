using System;
using System.Collections.Generic;
using Bloghost.Core.Entities;
using Bloghost.Core.Repositories;
using Bloghost.Core.Services.Interfaces;

namespace Bloghost.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [UnitOfWork]
        public IEnumerable<Comment> GetCommentList()
        {
            return _commentRepository.GetAll();
        }

        [UnitOfWork]
        public Comment GetComment(Guid id)
        {
            return _commentRepository.Get(id);
        }

        [UnitOfWork]
        public void CreateComment(Comment comment)
        {
            _commentRepository.Create(comment);
        }

        [UnitOfWork]
        public void UpdateComment(Comment comment)
        {
            _commentRepository.Update(comment);
        }

        [UnitOfWork]
        public void DeleteComment(System.Guid id)
        {
            _commentRepository.Delete(id);
        }
    }
}
