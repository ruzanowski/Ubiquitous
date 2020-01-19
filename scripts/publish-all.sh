#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
PREFIX=..\src
SERVICE=$PREFIX.Services
ADAPTER=$PREFIX.Adapters
BUILDINGBLOCK=$PREFIX.BuldingBlocks
REPOSITORIES=(
 $BUILDINGBLOCK\Common
 $BUILDINGBLOCK\U.EventBus
 $BUILDINGBLOCK\U.EventBus.RabbitMQ
 $BUILDINGBLOCK\U.IntegrationEventLog
 $PREFIX\ApiGateway
 $Adapters\SmartStore\U.SmartStoreAdapter
 $SERVICE\Fetch\U.FetchService
 $SERVICE\Generator\U.GeneratorService
 $SERVICE\Identity\U.IdentityService
 $SERVICE\Notification\U.NotificationService
 $SERVICE\Product\U.ProductService
 $SERVICE\Subscription\U.SubscriptionService)

for REPOSITORY in ${REPOSITORIES[*]}
do
	 echo ******************************************
	 echo Publishing a project: $REPOSITORY
	 echo ******************************************
	 cd $REPOSITORY
	 ./scripts/dotnet-publish.sh
	 cd ..
done