﻿using System;
using System.Linq.Expressions;
using FluentValidation.Results;

namespace VFi.NetDevPack.SpecificationResult;

internal sealed class OrSpecificationValidator<T> : SpecificationValidator<T>
where T : class
{
    private readonly SpecificationValidator<T> _left;
    private readonly SpecificationValidator<T> _right;

    public OrSpecificationValidator(SpecificationValidator<T> left, SpecificationValidator<T> right)
    {
        _right = right;
        _left = left;
    }

    public override Expression<Func<T, ValidationResult, bool>> ToExpression()
    {
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        var invokedExpression = Expression.Invoke(rightExpression, leftExpression.Parameters);

        return (Expression<Func<T, ValidationResult, bool>>)Expression.Lambda(Expression.OrElse(leftExpression.Body, invokedExpression), leftExpression.Parameters);
    }
}
