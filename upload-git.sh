#! /usr/bin/bash

# This function can be run afterwards
push_code() {
    cp -r ./${APP_DIRECTORY_NAME}/* ${GIT_DIRECTORY_NAME}
    cd ./${GIT_DIRECTORY_NAME}
    git add *
    git commit -m "Update: pushed files up for web app"
    git push -u origin master
}

######################################## Begin Script ########################################
GIT_DIRECTORY_NAME="git-app-code"
APP_DIRECTORY_NAME="todo-dotnet-app"

output=$(terraform output -json | jq '.username.value as $username | .password.value as $password | .git_clone_uri.value as $git_clone_uri | {username: $username, password: $password, git_clone_uri: $git_clone_uri}')
username=$(echo $output | jq --raw-output '.username')
password=$(echo $output | jq --raw-output '.password')
git_clone_uri=$(echo $output | jq --raw-output '.git_clone_uri'| sed "s#https://#https://${username}:${password}@#g")

# Put it in credential store if not exist
credential_store=$(echo $git_clone_uri | grep -Po 'https://.*?/' | sed 's#/$##g')

if [[ ! -e "${HOME}/.git-credentials" ]]; then
    echo -e "\e[93m${HOME}/.git-credentials does not exist. Creating...\e[97m"
    touch "${HOME}/.git-credentials"
fi 

if ! grep "${credential_store}" "${HOME}/.git-credentials" > /dev/null; then
    echo -e "\e[93mUpdating credential store with credentials...\e[97m"
    echo "${credential_store}" >> "${HOME}/.git-credentials"
fi

if [[ ! -d "${GIT_DIRECTORY_NAME}" ]]; then
    git clone "${git_clone_uri}" "${GIT_DIRECTORY_NAME}"
fi

# Uncomment this for CI-CD pipeline with ephemeral agent.
# push_code

