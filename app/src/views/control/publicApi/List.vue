<template>
  <a-card :bordered="false">
    <!--搜索-->
    <div class="table-page-search-wrapper">
      <a-row type="flex" justify="space-around" :gutter="48">
        <a-col :md="18" :sm="24">
          <a-tooltip>
            <template slot="title">新增公共 API</template>
            <a-button v-action:create type="primary" @click="$refs.add.show()" icon="plus" class="right10"></a-button>
          </a-tooltip>
          <a-select placeholder="请选择" v-model="queryParam.enable" style="width:100px;" @change="loadDataing">
            <a-select-option :value="-1">API 状态</a-select-option>
            <a-select-option :value="1">启用</a-select-option>
            <a-select-option :value="0">禁用</a-select-option>
          </a-select>
        </a-col>
        <a-col :md="6" :sm="24">
          <div class="table-page-search-submitButtons" >
            <a-input-search
              placeholder="公共API标识、名称"
              v-model="queryParam.vague"
              enterButton="查询"
              @search="loadDataing" >
            </a-input-search>
          </div>
        </a-col>
      </a-row>
    </div>

    <!--列表-->
    <a-table
      ref="table"
      :pagination="false"
      rowKey="id"
      :loading="table.loading"
      :columns="table.columns"
      :dataSource="table.dataSource">
      <span slot="createTime" slot-scope="text">
        {{ text | dayFormat('YYYY-MM-DD HH:mm') }}
      </span>
      <span slot="apiTemplate" slot-scope="text">
        <ellipsis :length="35" tooltip>
          {{ text }}
        </ellipsis>
      </span>
      <span slot="enable" slot-scope="text">
        <a-badge v-if="text" status="success" :text="text | statusFilter"/>
        <a-badge v-else status="error" :text="text | statusFilter"/>
      </span>
      <span slot="description" slot-scope="text">
        <ellipsis :length="80" tooltip>
          {{ text }}
        </ellipsis>
      </span>
      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="$refs.detail.show(record)">详情</a>
          <a-divider type="vertical" />
          <a-dropdown>
            <a class="ant-dropdown-link">更多
              <a-icon type="down" />
            </a>
            <a-menu slot="overlay">
              <a-menu-item v-action:modify :disabled="record.isSystem" @click="$refs.edit.show(record)">
                编辑
              </a-menu-item>
              <a-menu-item v-action:delete :disabled="record.isSystem" @click="handleDelete(record.id,record.name)" >
                删除
              </a-menu-item>
            </a-menu>
          </a-dropdown>
        </template>
      </span>
    </a-table>

    <!--功能模块-->
    <public-api-add ref="add" @complete="loadDataing"></public-api-add>
    <public-api-edit ref="edit" @complete="loadDataing"></public-api-edit>
    <public-api-detail ref="detail"></public-api-detail>
  </a-card>
</template>

<script>
import { STable, Ellipsis } from '@/components'
import { getPublicApiList, deletePublicApi } from '@/api/control'
import PublicApiAdd from './Add'
import PublicApiEdit from './Edit'
import PublicApiDetail from './Detail'

export default {
  name: 'PublicApiList',
  data () {
    return {
      // 查询参数
      queryParam: {
        vague: '',
        enable: -1
      },
      // 列表属性
      table: {
        loading: false,
        dataSource: [],
        columns: [
          {
            title: '标识',
            dataIndex: 'id',
            width: 100
          },
          {
            title: '名称',
            width: 200,
            dataIndex: 'name'
          },

          {
            title: '状态',
            align: 'center',
            width: 80,
            dataIndex: 'enable',
            scopedSlots: {
              customRender: 'enable'
            }
          },
          {
            title: 'Api 地址',
            dataIndex: 'apiTemplate',
            scopedSlots: {
              customRender: 'apiTemplate'
            }
          },
          {
            title: '描述',
            dataIndex: 'description',
            scopedSlots: {
              customRender: 'description'
            }
          },
          {
            title: '创建时间',
            width: 150,
            dataIndex: 'createTime',
            scopedSlots: {
              customRender: 'createTime'
            }
          },
          {
            title: '操作',
            width: 150,
            dataIndex: 'action',
            scopedSlots: {
              customRender: 'action'
            }
          }
        ]
      }
    }
  },
  components: {
    STable, PublicApiAdd, PublicApiEdit, PublicApiDetail, Ellipsis
  },
  methods: {
    /**
     * 加载列表
     */
    loadDataing () {
      this.table.loading = true
      getPublicApiList(this.queryParam).then(res => {
        this.table.loading = false
        if (res.status === 200) {
          this.table.dataSource = res.result
        } else {
          this.$notification.error({ message: '加载失败', description: res.message })
        }
      }).catch(() => {
        this.table.loading = false
      })
    },
    /**
     * 删除 公共 API
     *@param {String} id 编号
     *@param {String} name 名称
     */
    handleDelete (id, name) {
      const _this = this
      this.$confirm({ title: '删除公共API',
        content: `是否删除 ${name} 公共API？`,
        onOk: () => {
          deletePublicApi(id).then(res => {
            if (res.status === 200) {
              this.loadDataing()
              _this.$message.success(`删除 ${name} 公共API成功`)
            } else {
              _this.$message.error('删除失败;' + res.message)
            }
          })
        }
      })
    }
  },
  created () {
    this.loadDataing()
  }
}
</script>

<style lang="less" scoped>
.right10 {
   margin-right:10px;
}
.operator {
  margin-bottom: 18px;
}
@media screen and (max-width: 900px) {
  .fold { width: 100%;}
}
</style>
